namespace Application.Interfaces
{
    public interface ICryptoService
    {
        string HashPassword(string password);
        bool VerifyPassword(string password, string hash);
    }
}