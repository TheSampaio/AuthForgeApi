using System.ComponentModel.DataAnnotations;

namespace Application.Contracts
{
    public record SsoLoginRequest(
        [Required(AllowEmptyStrings = false), EmailAddress] string Email,
        [Required(AllowEmptyStrings = false)] string Password,
        Guid ClientId
    );
}