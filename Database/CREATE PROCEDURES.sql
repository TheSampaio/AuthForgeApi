USE AuthForge;
GO

-- ==========================================
-- 1. SP: Upsert User
-- ==========================================
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

        -- Audit Log
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

-- ==========================================
-- 2. SP: Upsert Account
-- ==========================================
CREATE PROCEDURE sp_UpsertAccount
    @Id INT = NULL,
    @UserId INT,
    @AccountName NVARCHAR(64),
    @AccountType TINYINT,
    @CurrentBalance DECIMAL(19, 4),
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

            INSERT INTO Accounts (UserId, AccountName, AccountType, CurrentBalance, IsActive, CreatedAtUtc)
            VALUES (@UserId, @AccountName, @AccountType, @CurrentBalance, @IsActive, GETUTCDATE());

            SET @CurrentId = SCOPE_IDENTITY();
        END
        ELSE
        BEGIN
            SET @OperationType = 'UPDATE';

            UPDATE Accounts
            SET UserId = @UserId,
                AccountName = @AccountName,
                AccountType = @AccountType,
                CurrentBalance = @CurrentBalance,
                IsActive = @IsActive
            WHERE Id = @CurrentId;
        END

        -- Audit Log
        INSERT INTO AccountsLog (RecordId, UserId, AccountName, AccountType, CurrentBalance, OperationUserId, OperationType, IsActive, CreatedAtUtc)
        VALUES (@CurrentId, @UserId, @AccountName, @AccountType, @CurrentBalance, @OperationUserId, @OperationType, @IsActive, GETUTCDATE());

        COMMIT TRANSACTION;
        SELECT @CurrentId AS RecordId;

    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END
GO

-- ==========================================
-- 3. SP: Upsert Category
-- ==========================================
CREATE PROCEDURE sp_UpsertCategory
    @Id INT = NULL,
    @UserId INT,
    @Name NVARCHAR(64),
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

            INSERT INTO Categories (UserId, Name, IsActive, CreatedAtUtc)
            VALUES (@UserId, @Name, @IsActive, GETUTCDATE());

            SET @CurrentId = SCOPE_IDENTITY();
        END
        ELSE
        BEGIN
            SET @OperationType = 'UPDATE';

            UPDATE Categories
            SET UserId = @UserId,
                Name = @Name,
                IsActive = @IsActive
            WHERE Id = @CurrentId;
        END

        -- Audit Log
        INSERT INTO CategoriesLog (RecordId, UserId, Name, OperationUserId, OperationType, IsActive, CreatedAtUtc)
        VALUES (@CurrentId, @UserId, @Name, @OperationUserId, @OperationType, @IsActive, GETUTCDATE());

        COMMIT TRANSACTION;
        SELECT @CurrentId AS RecordId;

    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END
GO

-- ==========================================
-- 4. SP: Upsert Transaction
-- ==========================================
CREATE PROCEDURE sp_UpsertTransaction
    @Id BIGINT = NULL,
    @AccountId INT,
    @CategoryId INT = NULL,
    @Amount DECIMAL(19, 4),
    @Title NVARCHAR(128),
    @TransactionDate DATETIME2,
    @Reference NVARCHAR(128) = NULL,
    @IsActive BIT = 1,
    @OperationUserId INT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;

        DECLARE @OperationType NVARCHAR(16);
        DECLARE @CurrentId BIGINT = @Id;

        -- Resolve by Reference if an ID isn't provided but a reference exists
        IF (@CurrentId IS NULL OR @CurrentId = 0) AND @Reference IS NOT NULL
        BEGIN
            SELECT @CurrentId = Id 
            FROM Transactions 
            WHERE Reference = @Reference;
        END

        IF @CurrentId IS NULL OR @CurrentId = 0
        BEGIN
            SET @OperationType = 'INSERT';

            INSERT INTO Transactions (AccountId, CategoryId, Amount, Title, TransactionDate, Reference, IsActive, CreatedAtUtc)
            VALUES (@AccountId, @CategoryId, @Amount, @Title, @TransactionDate, @Reference, @IsActive, GETUTCDATE());

            SET @CurrentId = SCOPE_IDENTITY();
        END
        ELSE
        BEGIN
            SET @OperationType = 'UPDATE';

            UPDATE Transactions
            SET AccountId = @AccountId,
                CategoryId = @CategoryId,
                Amount = @Amount,
                Title = @Title,
                TransactionDate = @TransactionDate,
                Reference = @Reference,
                IsActive = @IsActive
            WHERE Id = @CurrentId;
        END

        -- Audit Log
        INSERT INTO TransactionsLog (RecordId, AccountId, CategoryId, Amount, Title, TransactionDate, Reference, OperationUserId, OperationType, IsActive, CreatedAtUtc)
        VALUES (@CurrentId, @AccountId, @CategoryId, @Amount, @Title, @TransactionDate, @Reference, @OperationUserId, @OperationType, @IsActive, GETUTCDATE());

        COMMIT TRANSACTION;
        SELECT @CurrentId AS RecordId;

    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END
GO