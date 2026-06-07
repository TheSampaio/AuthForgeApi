CREATE DATABASE AuthForge;
GO

USE AuthForge;
GO

-- ==========================================
-- 1. BASE TABLES
-- ==========================================

-- Users Table
CREATE TABLE Users
(
    [Id] INT PRIMARY KEY IDENTITY(1, 1),
    [FirstName] NVARCHAR(64) NOT NULL,
    [LastName] NVARCHAR(64) NOT NULL,
    [Email] NVARCHAR(256) NOT NULL UNIQUE,
    [PasswordHash] NVARCHAR(512) NOT NULL,
    [Birthdate] DATETIME2 NOT NULL,
    [IsActive] BIT NOT NULL DEFAULT 1,
    [CreatedAtUtc] DATETIME2 NOT NULL DEFAULT GETUTCDATE()
);
GO

-- Accounts Table (Checking, Savings, Credit Cards)
CREATE TABLE Accounts
(
    [Id] INT PRIMARY KEY IDENTITY(1, 1),
    [UserId] INT NOT NULL,
    [AccountName] NVARCHAR(64) NOT NULL, 
    [AccountType] TINYINT NOT NULL DEFAULT 1, 
    [CurrentBalance] DECIMAL(19, 4) NOT NULL DEFAULT 0.00,
    [IsActive] BIT NOT NULL DEFAULT 1,
    [CreatedAtUtc] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),

    CONSTRAINT FK_Accounts_Users FOREIGN KEY (UserId) REFERENCES Users([Id]) ON DELETE CASCADE
);
GO

CREATE NONCLUSTERED INDEX IX_Accounts_UserId ON Accounts(UserId);
GO

-- Categories Table
CREATE TABLE Categories
(
    [Id] INT PRIMARY KEY IDENTITY(1, 1),
    [UserId] INT NOT NULL,
    [Name] NVARCHAR(64) NOT NULL, 
    [IsActive] BIT NOT NULL DEFAULT 1,
    [CreatedAtUtc] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),

    CONSTRAINT FK_Categories_Users FOREIGN KEY (UserId) REFERENCES Users([Id])
);
GO

CREATE NONCLUSTERED INDEX IX_Categories_UserId ON Categories(UserId);
GO

-- Transactions Table
CREATE TABLE Transactions
(
    [Id] BIGINT PRIMARY KEY IDENTITY(1, 1), 
    [AccountId] INT NOT NULL,
    [CategoryId] INT NULL, 
    [Amount] DECIMAL(19, 4) NOT NULL, 
    [Title] NVARCHAR(128) NOT NULL,
    [TransactionDate] DATETIME2 NOT NULL,
    [Reference] NVARCHAR(128) NULL, 
    [IsActive] BIT NOT NULL DEFAULT 1, 
    [CreatedAtUtc] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),

    CONSTRAINT FK_Transactions_Accounts FOREIGN KEY (AccountId) REFERENCES Accounts([Id]) ON DELETE CASCADE,
    CONSTRAINT FK_Transactions_Categories FOREIGN KEY (CategoryId) REFERENCES Categories([Id])
);
GO

CREATE NONCLUSTERED INDEX IX_Transactions_AccountId_Date ON Transactions(AccountId, TransactionDate);
GO

CREATE NONCLUSTERED INDEX IX_Transactions_Reference ON Transactions(Reference);
GO

-- ==========================================
-- 2. LOG TABLES
-- ==========================================

-- Users Log
CREATE TABLE UsersLog
(
    [Id] BIGINT PRIMARY KEY IDENTITY(1, 1),
    [RecordId] INT NOT NULL, 
    [FirstName] NVARCHAR(64) NULL,
    [LastName] NVARCHAR(64) NULL,
    [Email] NVARCHAR(256) NULL,
    [PasswordHash] NVARCHAR(512) NULL,
    [Birthdate] DATETIME2 NULL,
    [OperationUserId] INT NOT NULL, 
    [OperationType] NVARCHAR(16) NOT NULL, 
    [IsActive] BIT NOT NULL DEFAULT 1,
    [CreatedAtUtc] DATETIME2 NOT NULL DEFAULT GETUTCDATE()
);
GO

CREATE NONCLUSTERED INDEX IX_UsersLog_RecordId ON UsersLog(RecordId);
GO

-- Accounts Log
CREATE TABLE AccountsLog
(
    [Id] BIGINT PRIMARY KEY IDENTITY(1, 1),
    [RecordId] INT NOT NULL, 
    [UserId] INT NULL,
    [AccountName] NVARCHAR(64) NULL, 
    [AccountType] TINYINT NULL, 
    [CurrentBalance] DECIMAL(19, 4) NULL,
    [OperationUserId] INT NOT NULL, 
    [OperationType] NVARCHAR(16) NOT NULL, 
    [IsActive] BIT NOT NULL DEFAULT 1,
    [CreatedAtUtc] DATETIME2 NOT NULL DEFAULT GETUTCDATE()
);
GO

CREATE NONCLUSTERED INDEX IX_AccountsLog_RecordId ON AccountsLog(RecordId);
GO

-- Categories Log
CREATE TABLE CategoriesLog
(
    [Id] BIGINT PRIMARY KEY IDENTITY(1, 1),
    [RecordId] INT NOT NULL, 
    [UserId] INT NULL,
    [Name] NVARCHAR(64) NULL,
    [OperationUserId] INT NOT NULL, 
    [OperationType] NVARCHAR(16) NOT NULL, 
    [IsActive] BIT NOT NULL DEFAULT 1,
    [CreatedAtUtc] DATETIME2 NOT NULL DEFAULT GETUTCDATE()
);
GO

CREATE NONCLUSTERED INDEX IX_CategoriesLog_RecordId ON CategoriesLog(RecordId);
GO

-- Transactions Log
CREATE TABLE TransactionsLog
(
    [Id] BIGINT PRIMARY KEY IDENTITY(1, 1),
    [RecordId] BIGINT NOT NULL, 
    [AccountId] INT NULL,
    [CategoryId] INT NULL, 
    [Amount] DECIMAL(19, 4) NULL, 
    [Title] NVARCHAR(128) NULL,
    [TransactionDate] DATETIME2 NULL,
    [Reference] NVARCHAR(128) NULL, 
    [OperationUserId] INT NOT NULL, 
    [OperationType] NVARCHAR(16) NOT NULL, 
    [IsActive] BIT NOT NULL DEFAULT 1,
    [CreatedAtUtc] DATETIME2 NOT NULL DEFAULT GETUTCDATE()
);
GO

CREATE NONCLUSTERED INDEX IX_TransactionsLog_RecordId ON TransactionsLog(RecordId);
GO