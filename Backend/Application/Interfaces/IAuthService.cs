using Application.Contracts;

namespace Application.Interfaces
{
    public interface IAuthService
    {
        Task<Result<int>> RegisterAsync(RegisterRequest request);
        Task<Result<LoginResponse>> LoginAsync(LoginRequest request);
    }
}