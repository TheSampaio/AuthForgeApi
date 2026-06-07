namespace Application.Contracts
{
    public record UserResponse
    (
        int Id,
        string FirstName,
        string LastName,
        string Email
    );
}
