namespace Infrastructure.Statements
{
    public class UsersStatements
    {
        public const string SelectAll = @"
            SELECT
                Id,
                FirstName,
                LastName,
                Email,
                PasswordHash,
                Birthdate,
                IsActive,
                CreatedAtUtc
            FROM
                Users
            WHERE
                IsActive = 1";

        public const string SelectById = @"
            SELECT
                Id,
                FirstName,
                LastName,
                Email,
                PasswordHash,
                Birthdate,
                IsActive,
                CreatedAtUtc
            FROM
                Users
            WHERE
                IsActive = 1
                AND Id = @Id";

        public const string SelectByEmail = @"
            SELECT
                Id,
                FirstName,
                LastName,
                Email,
                PasswordHash,
                Birthdate,
                IsActive,
                CreatedAtUtc
            FROM
                Users
            WHERE
                IsActive = 1
                AND Email = @Email";
    }
}
