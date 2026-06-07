namespace Application.Contracts
{
    public record RegisterRequest
    (
        string FirstName,
        string LastName,
        string Email,
        string Password,
        DateTime Birthdate
    );
}