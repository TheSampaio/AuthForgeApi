using Application.Contracts;

namespace Application.Interfaces
{
    public interface ISsoService
    {
        Task<Result<LoginResponse>> LoginAsync(SsoLoginRequest request);
        Task<Result<LoginResponse>> RegisterAsync(SsoRegisterRequest request);
    }
}