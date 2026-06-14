using System.ComponentModel.DataAnnotations;

namespace Application.Contracts
{
    public record CreateApplicationRequest(
        [Required(AllowEmptyStrings = false)] string Name
    );
}