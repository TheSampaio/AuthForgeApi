namespace Infrastructure.Statements
{
    public class UsersStatements
    {
        public const string SelectAll = @"
            SELECT
                *
            FROM
                Users
            WHERE
                IsActive = 1";

        public const string SelectById = @"
            SELECT
                *
            FROM
                Users
            WHERE
                IsActive = 1
                AND Id = @Id";
    }
}
