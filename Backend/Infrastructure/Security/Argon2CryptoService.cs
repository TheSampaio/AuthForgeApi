using Application.Interfaces;
using Konscious.Security.Cryptography;
using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;
using System.Text;

namespace Infrastructure.Security
{
    public class Argon2CryptoService : ICryptoService
    {
        private const int DegreeOfParallelism = 8;
        private const int Iterations = 4;
        private const int MemorySize = 1024 * 128; // 128 MB
        private readonly byte[] _pepper;

        public Argon2CryptoService(IConfiguration configuration)
        {
            var pepperString = configuration["CryptoSettings:Pepper"]
                ?? throw new InvalidOperationException("Crypto Pepper missing.");

            _pepper = Encoding.UTF8.GetBytes(pepperString);
        }

        public string HashPassword(string password)
        {
            var salt = CreateSalt();
            var hash = GenerateHash(password, salt);

            return $"{Convert.ToBase64String(salt)}:{Convert.ToBase64String(hash)}";
        }

        public bool VerifyPassword(string password, string hash)
        {
            var parts = hash.Split(':');

            if (parts.Length != 2)
                return false;

            var salt = Convert.FromBase64String(parts[0]);
            var expectedHash = Convert.FromBase64String(parts[1]);
            var actualHash = GenerateHash(password, salt);

            return CryptographicOperations.FixedTimeEquals(expectedHash, actualHash);
        }

        private static byte[] CreateSalt()
        {
            var buffer = new byte[16];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(buffer);
            return buffer;
        }

        private byte[] GenerateHash(string password, byte[] salt)
        {
            using var argon2 = new Argon2id(Encoding.UTF8.GetBytes(password))
            {
                Salt = salt,
                DegreeOfParallelism = DegreeOfParallelism,
                Iterations = Iterations,
                MemorySize = MemorySize,
                KnownSecret = _pepper
            };

            return argon2.GetBytes(32);
        }
    }
}