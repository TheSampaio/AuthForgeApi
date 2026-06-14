namespace Infrastructure.Statements
{
    public class UserApplicationsStatements
    {
        public const string SelectGrant = @"
            SELECT
                Id,
                UserId,
                ApplicationId,
                Roles,
                IsActive,
                CreatedAtUtc
            FROM
                UserApplications
            WHERE
                IsActive = 1
                AND UserId = @UserId
                AND ApplicationId = @ApplicationId";

        public const string UpsertUserApplication = "sp_UpsertUserApplication";
    }
}