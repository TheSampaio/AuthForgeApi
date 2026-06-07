namespace Application.Contracts
{
    public record LoginResponse
    (
        string Email,
        string Token,
        int ExpiresInMinutes
    );
}