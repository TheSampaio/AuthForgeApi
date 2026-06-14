using System.ComponentModel.DataAnnotations;

namespace Application.Contracts
{
    public record AssignUserRequest(
        int UserId,
        Guid ClientId,
        [Required(AllowEmptyStrings = false)] string Role
    );
}