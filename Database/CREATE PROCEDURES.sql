USE AuthForge;
GO

-- ==============================
-- sp_UpsertUser
-- ==============================
CREATE PROCEDURE sp_UpsertUser
    @Id INT = NULL,
    @FirstName NVARCHAR(64),
    @LastName NVARCHAR(64),
    @Email NVARCHAR(256),
    @PasswordHash NVARCHAR(512),
    @Birthdate DATETIME2,
    @IsActive BIT = 1,
    @OperationUserId INT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;

        DECLARE @OperationType NVARCHAR(16);
        DECLARE @CurrentId INT = @Id;

        IF @CurrentId IS NULL OR @CurrentId = 0
        BEGIN
            SET @OperationType = 'INSERT';

            INSERT INTO Users (FirstName, LastName, Email, PasswordHash, Birthdate, IsActive, CreatedAtUtc)
            VALUES (@FirstName, @LastName, @Email, @PasswordHash, @Birthdate, @IsActive, GETUTCDATE());

            SET @CurrentId = SCOPE_IDENTITY();
        END
        ELSE
        BEGIN
            SET @OperationType = 'UPDATE';

            UPDATE Users
            SET FirstName = @FirstName,
                LastName = @LastName,
                Email = @Email,
                PasswordHash = @PasswordHash,
                Birthdate = @Birthdate,
                IsActive = @IsActive
            WHERE Id = @CurrentId;
        END

        INSERT INTO UsersLog (RecordId, FirstName, LastName, Email, PasswordHash, Birthdate, OperationUserId, OperationType, IsActive, CreatedAtUtc)
        VALUES (@CurrentId, @FirstName, @LastName, @Email, @PasswordHash, @Birthdate, @OperationUserId, @OperationType, @IsActive, GETUTCDATE());

        COMMIT TRANSACTION;
        SELECT @CurrentId AS RecordId;

    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END
GO

-- ==============================
-- sp_UpsertApplication
-- ==============================
CREATE PROCEDURE sp_UpsertApplication
    @Id INT = NULL,
    @Name NVARCHAR(64),
    @ClientId UNIQUEIDENTIFIER = NULL,
    @ClientSecret NVARCHAR(256) = NULL,
    @IsActive BIT = 1,
    @OperationUserId INT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;

        DECLARE @OperationType NVARCHAR(16);
        DECLARE @CurrentId INT = @Id;

        IF (@CurrentId IS NULL OR @CurrentId = 0) AND @ClientId IS NULL
            SET @ClientId = NEWID();

        IF @CurrentId IS NULL OR @CurrentId = 0
        BEGIN
            SET @OperationType = 'INSERT';
            INSERT INTO Applications (Name, ClientId, ClientSecret, IsActive, CreatedAtUtc)
            VALUES (@Name, @ClientId, @ClientSecret, @IsActive, GETUTCDATE());
            SET @CurrentId = SCOPE_IDENTITY();
        END
        ELSE
        BEGIN
            SET @OperationType = 'UPDATE';
            UPDATE Applications
            SET Name = @Name,
                ClientId = ISNULL(@ClientId, ClientId), 
                ClientSecret = @ClientSecret,
                IsActive = @IsActive
            WHERE Id = @CurrentId;
            
            IF @ClientId IS NULL
                SELECT @ClientId = ClientId FROM Applications WHERE Id = @CurrentId;
        END

        INSERT INTO ApplicationsLog (RecordId, Name, ClientId, ClientSecret, OperationUserId, OperationType, IsActive, CreatedAtUtc)
        VALUES (@CurrentId, @Name, @ClientId, @ClientSecret, @OperationUserId, @OperationType, @IsActive, GETUTCDATE());

        COMMIT TRANSACTION;
        SELECT @CurrentId AS RecordId;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END
GO

-- ==============================
-- sp_UpsertUserApplication
-- ==============================
CREATE PROCEDURE sp_UpsertUserApplication
    @Id INT = NULL,
    @UserId INT,
    @ApplicationId INT,
    @Roles NVARCHAR(256),
    @IsActive BIT = 1,
    @OperationUserId INT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;

        DECLARE @OperationType NVARCHAR(16);
        DECLARE @CurrentId INT = @Id;

        IF @CurrentId IS NULL OR @CurrentId = 0
        BEGIN
            SET @OperationType = 'INSERT';
            INSERT INTO UserApplications (UserId, ApplicationId, Roles, IsActive, CreatedAtUtc)
            VALUES (@UserId, @ApplicationId, @Roles, @IsActive, GETUTCDATE());
            SET @CurrentId = SCOPE_IDENTITY();
        END
        ELSE
        BEGIN
            SET @OperationType = 'UPDATE';
            UPDATE UserApplications
            SET UserId = @UserId,
                ApplicationId = @ApplicationId,
                Roles = @Roles,
                IsActive = @IsActive
            WHERE Id = @CurrentId;
        END

        INSERT INTO UserApplicationsLog (RecordId, UserId, ApplicationId, Roles, OperationUserId, OperationType, IsActive, CreatedAtUtc)
        VALUES (@CurrentId, @UserId, @ApplicationId, @Roles, @OperationUserId, @OperationType, @IsActive, GETUTCDATE());

        COMMIT TRANSACTION;
        SELECT @CurrentId AS RecordId;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END
GO