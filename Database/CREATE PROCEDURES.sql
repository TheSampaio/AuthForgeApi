USE AuthForge;
GO

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