using System.ComponentModel.DataAnnotations;

namespace Application.Contracts
{
    public record SsoRegisterRequest(
        [Required(AllowEmptyStrings = false)] string FirstName,
        [Required(AllowEmptyStrings = false)] string LastName,
        [Required(AllowEmptyStrings = false), EmailAddress] string Email,
        [Required(AllowEmptyStrings = false)] string Password,
        DateTime Birthdate,
        Guid ClientId
    );
}