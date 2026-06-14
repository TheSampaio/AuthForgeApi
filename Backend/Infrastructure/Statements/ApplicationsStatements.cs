namespace Infrastructure.Statements
{
    public class ApplicationsStatements
    {
        public const string SelectById = @"
            SELECT
                Id,
                Name,
                ClientId,
                ClientSecret,
                IsActive,
                CreatedAtUtc
            FROM
                Applications 
            WHERE
                IsActive = 1
                AND Id = @Id";

        public const string SelectByClientId = @"
            SELECT
                Id,
                Name,
                ClientId,
                ClientSecret,
                IsActive,
                CreatedAtUtc
            FROM
                Applications 
            WHERE
                IsActive = 1
                AND ClientId = @ClientId";

        public const string SelectClientIdById = @"
            SELECT
                ClientId
            FROM
                Applications
            WHERE
                Id = @Id";

        public const string SelectByUserId = @"
            SELECT 
                A.Id, 
                A.Name, 
                A.ClientId, 
                A.IsActive 
            FROM 
                Applications A
                INNER JOIN UserApplications UA ON A.Id = UA.ApplicationId
            WHERE 
                A.IsActive = 1 
                AND UA.IsActive = 1 
                AND UA.UserId = @UserId 
                AND UA.Roles LIKE '%Admin%'";

        public const string UpsertApplication = "sp_UpsertApplication";
    }
}