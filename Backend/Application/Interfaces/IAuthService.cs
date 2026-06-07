using Application.Contracts;

namespace Application.Interfaces
{
    public interface IAuthService
    {
        Task<int> RegisterAsync(RegisterRequest request);
        Task<LoginResponse?> LoginAsync(LoginRequest request);
    }
}