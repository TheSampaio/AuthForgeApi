USE AuthForge;
GO

DECLARE @DefaultHash NVARCHAR(512) = '$argon2id$v=19$m=65536,t=3,p=1$c29tZXNhbHQ$RummyHashFor1234MockData';
DECLARE @Now DATETIME2 = GETUTCDATE();

SET IDENTITY_INSERT Users ON;
INSERT INTO Users (Id, FirstName, LastName, Email, PasswordHash, Birthdate, IsActive, CreatedAtUtc)
VALUES 
    (1, 'Kellvyn', 'Sampaio', 'kellvyn.sampaio@test.com', @DefaultHash, '1990-05-15', 1, @Now),
    (2, 'Linus', 'Torvalds', 'linus.torvalds@test.com', @DefaultHash, '1969-12-28', 1, @Now),
    (3, 'Ada', 'Lovelace', 'ada.lovelace@test.com', @DefaultHash, '1815-12-10', 1, @Now);
SET IDENTITY_INSERT Users OFF;
GO