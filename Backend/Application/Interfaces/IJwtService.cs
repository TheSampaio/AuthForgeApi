using Domain.Entities;

namespace Application.Interfaces
{
    public interface IJwtService
    {
        string GenerateToken(UsersEntity user, string? audience = null, string? roles = null);
    }
}