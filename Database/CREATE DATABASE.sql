CREATE DATABASE AuthForge;
GO

USE AuthForge;
GO

-- ==========================================
-- 1. BASE TABLES
-- ==========================================

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

-- ==========================================
-- 2. LOG TABLES
-- ==========================================

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