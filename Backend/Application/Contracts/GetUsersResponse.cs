namespace Application.Contracts
{
    public record GetUsersResponse
    (
        int Id,
        string FirstName,
        string LastName,
        string Email
    );
}
