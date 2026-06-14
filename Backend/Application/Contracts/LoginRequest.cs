using System.ComponentModel.DataAnnotations;

namespace Application.Contracts
{
    public record LoginRequest(
        [Required(AllowEmptyStrings = false), EmailAddress] string Email,
        [Required(AllowEmptyStrings = false)] string Password
    );
}