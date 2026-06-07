USE AuthForge;
GO

-- Seed script for mock data
-- Default password: '1234'
DECLARE @DefaultHash NVARCHAR(512) = '$argon2id$v=19$m=65536,t=3,p=1$c29tZXNhbHQ$RummyHashFor1234MockData';
DECLARE @Now DATETIME2 = GETUTCDATE();

-- Users
SET IDENTITY_INSERT Users ON;
INSERT INTO Users (Id, FirstName, LastName, Email, PasswordHash, Birthdate, IsActive, CreatedAtUtc)
VALUES 
    (1, 'Kellvyn', 'Sampaio', 'kellvyn@test.com', @DefaultHash, '1990-05-15', 1, @Now),
    (2, 'Linus', 'Torvalds', 'linus@test.com', @DefaultHash, '1969-12-28', 1, @Now),
    (3, 'Ada', 'Lovelace', 'ada@test.com', @DefaultHash, '1815-12-10', 1, @Now);
SET IDENTITY_INSERT Users OFF;

-- Categories
SET IDENTITY_INSERT Categories ON;
INSERT INTO Categories (Id, UserId, Name, IsActive, CreatedAtUtc) 
VALUES 
    (1, 1, 'Groceries', 1, @Now),
    (2, 1, 'Salary', 1, @Now),
    (3, 1, 'Subscriptions', 1, @Now);
SET IDENTITY_INSERT Categories OFF;

-- Accounts (AccountType: 1 = Checking/Savings, 2 = Credit Card)
SET IDENTITY_INSERT Accounts ON;
INSERT INTO Accounts (Id, UserId, AccountName, AccountType, CurrentBalance, IsActive, CreatedAtUtc)
VALUES 
    (1, 1, 'NuBank Checking', 1, 5500.00, 1, @Now),
    (2, 1, 'NuBank Credit Card', 2, -850.50, 1, @Now),
    (3, 2, 'Main Bank', 1, 12000.00, 1, @Now),
    (4, 3, 'Digital Wallet', 1, 340.75, 1, @Now);
SET IDENTITY_INSERT Accounts OFF;

-- Transactions
SET IDENTITY_INSERT Transactions ON;
INSERT INTO Transactions (Id, AccountId, CategoryId, Amount, Title, TransactionDate, Reference, IsActive, CreatedAtUtc)
VALUES 
    (1, 1, 2, 7000.00, 'Monthly Salary', @Now, 'PIX-SALARY-001', 1, @Now),
    (2, 1, 1, -450.00, 'Supermercado Volta Redonda', @Now, 'PIX-B.REFER-8839', 1, @Now),
    (3, 2, 3, -45.90, 'Netflix Subscription', @Now, 'CC-NETFLIX', 1, @Now),
    (4, 2, 1, -120.00, 'Ifood Dinner', @Now, 'CC-IFOOD', 1, @Now),
    (5, 3, NULL, -2000.00, 'Hardware Purchase', @Now, 'WIRE-TRANS-01', 1, @Now),
    (6, 4, NULL, 500.00, 'Freelance Payment', @Now, 'PIX-FREE-99', 1, @Now);
SET IDENTITY_INSERT Transactions OFF;
GO