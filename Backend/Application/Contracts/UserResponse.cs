namespace Application.Contracts
{
    public record UserResponse
    (
        string FirstName,
        string LastName,
        string Email,
        DateTime Birthdate
    );
}