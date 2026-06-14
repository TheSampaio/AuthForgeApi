CREATE DATABASE AuthForge;
GO

USE AuthForge;
GO

-- ==============================
-- BASE TABLES
-- ==============================

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

CREATE TABLE Applications
(
    [Id] INT PRIMARY KEY IDENTITY(1, 1),
    [Name] NVARCHAR(64) NOT NULL UNIQUE,
    [ClientId] UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
    [ClientSecret] NVARCHAR(256) NULL,
    [IsActive] BIT NOT NULL DEFAULT 1,
    [CreatedAtUtc] DATETIME2 NOT NULL DEFAULT GETUTCDATE()
);
GO

CREATE TABLE UserApplications
(
    [Id] INT PRIMARY KEY IDENTITY(1, 1),
    [UserId] INT NOT NULL,
    [ApplicationId] INT NOT NULL,
    [Roles] NVARCHAR(256) NULL,
    [IsActive] BIT NOT NULL DEFAULT 1,
    [CreatedAtUtc] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),

    CONSTRAINT UQ_UserApplications_User_App UNIQUE (UserId, ApplicationId),
    FOREIGN KEY (UserId) REFERENCES Users(Id),
    FOREIGN KEY (ApplicationId) REFERENCES Applications(Id)
);
GO

-- ==============================
-- LOG TABLES
-- ==============================

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

CREATE TABLE ApplicationsLog
(
    [Id] BIGINT PRIMARY KEY IDENTITY(1, 1),
    [RecordId] INT NOT NULL, 
    [Name] NVARCHAR(64) NULL,
    [ClientId] UNIQUEIDENTIFIER NULL,
    [ClientSecret] NVARCHAR(256) NULL,
    [OperationUserId] INT NOT NULL, 
    [OperationType] NVARCHAR(16) NOT NULL, 
    [IsActive] BIT NOT NULL DEFAULT 1,
    [CreatedAtUtc] DATETIME2 NOT NULL DEFAULT GETUTCDATE()
);
GO

CREATE TABLE UserApplicationsLog
(
    [Id] BIGINT PRIMARY KEY IDENTITY(1, 1),
    [RecordId] INT NOT NULL, 
    [UserId] INT NULL,
    [ApplicationId] INT NULL,
    [Roles] NVARCHAR(256) NULL,
    [OperationUserId] INT NOT NULL, 
    [OperationType] NVARCHAR(16) NOT NULL, 
    [IsActive] BIT NOT NULL DEFAULT 1,
    [CreatedAtUtc] DATETIME2 NOT NULL DEFAULT GETUTCDATE()
);
GO