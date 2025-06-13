IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [Agencies] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NOT NULL,
    [Address_Street] nvarchar(max) NOT NULL,
    [Address_City] nvarchar(max) NOT NULL,
    [Address_State] nvarchar(max) NOT NULL,
    [Address_ZipCode] nvarchar(max) NOT NULL,
    [ContactEmail] nvarchar(100) NOT NULL,
    CONSTRAINT [PK_Agencies] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [AspNetRoles] (
    [Id] uniqueidentifier NOT NULL,
    [Name] nvarchar(256) NULL,
    [NormalizedName] nvarchar(256) NULL,
    [ConcurrencyStamp] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetRoles] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [AspNetUsers] (
    [Id] uniqueidentifier NOT NULL,
    [Discriminator] nvarchar(21) NOT NULL,
    [AgencyId] int NULL,
    [UserName] nvarchar(256) NULL,
    [NormalizedUserName] nvarchar(256) NULL,
    [Email] nvarchar(256) NULL,
    [NormalizedEmail] nvarchar(256) NULL,
    [EmailConfirmed] bit NOT NULL,
    [PasswordHash] nvarchar(max) NULL,
    [SecurityStamp] nvarchar(max) NULL,
    [ConcurrencyStamp] nvarchar(max) NULL,
    [PhoneNumber] nvarchar(max) NULL,
    [PhoneNumberConfirmed] bit NOT NULL,
    [TwoFactorEnabled] bit NOT NULL,
    [LockoutEnd] datetimeoffset NULL,
    [LockoutEnabled] bit NOT NULL,
    [AccessFailedCount] int NOT NULL,
    CONSTRAINT [PK_AspNetUsers] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AspNetUsers_Agencies_AgencyId] FOREIGN KEY ([AgencyId]) REFERENCES [Agencies] ([Id]) ON DELETE NO ACTION
);
GO

CREATE TABLE [AspNetRoleClaims] (
    [Id] int NOT NULL IDENTITY,
    [RoleId] uniqueidentifier NOT NULL,
    [ClaimType] nvarchar(max) NULL,
    [ClaimValue] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetRoleClaims] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [AspNetUserClaims] (
    [Id] int NOT NULL IDENTITY,
    [UserId] uniqueidentifier NOT NULL,
    [ClaimType] nvarchar(max) NULL,
    [ClaimValue] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [AspNetUserLogins] (
    [LoginProvider] nvarchar(450) NOT NULL,
    [ProviderKey] nvarchar(450) NOT NULL,
    [ProviderDisplayName] nvarchar(max) NULL,
    [UserId] uniqueidentifier NOT NULL,
    CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY ([LoginProvider], [ProviderKey]),
    CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [AspNetUserRoles] (
    [UserId] uniqueidentifier NOT NULL,
    [RoleId] uniqueidentifier NOT NULL,
    CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY ([UserId], [RoleId]),
    CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [AspNetUserTokens] (
    [UserId] uniqueidentifier NOT NULL,
    [LoginProvider] nvarchar(450) NOT NULL,
    [Name] nvarchar(450) NOT NULL,
    [Value] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY ([UserId], [LoginProvider], [Name]),
    CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [Notifications] (
    [Id] int NOT NULL IDENTITY,
    [Message] nvarchar(max) NOT NULL,
    [SentAt] datetimeoffset NOT NULL,
    [IsRead] bit NOT NULL DEFAULT CAST(0 AS bit),
    [UserId] uniqueidentifier NOT NULL,
    CONSTRAINT [PK_Notifications] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Notifications_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [Properties] (
    [Id] uniqueidentifier NOT NULL,
    [Title] nvarchar(200) NOT NULL,
    [Description] nvarchar(max) NOT NULL,
    [Address_Street] nvarchar(max) NOT NULL,
    [Address_City] nvarchar(max) NOT NULL,
    [Address_State] nvarchar(max) NOT NULL,
    [Address_ZipCode] nvarchar(max) NOT NULL,
    [Location] nvarchar(max) NOT NULL,
    [Price] decimal(10,2) NOT NULL,
    [Status] int NOT NULL,
    [CreatedDate] datetimeoffset NOT NULL,
    [AgencyId] int NULL,
    [UserId] uniqueidentifier NULL,
    CONSTRAINT [PK_Properties] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Properties_Agencies_AgencyId] FOREIGN KEY ([AgencyId]) REFERENCES [Agencies] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Properties_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id])
);
GO

CREATE TABLE [SearchCriterias] (
    [Id] int NOT NULL IDENTITY,
    [Keywords] nvarchar(200) NOT NULL,
    [MinPrice] decimal(10,2) NOT NULL,
    [MaxPrice] decimal(10,2) NOT NULL,
    [Location] nvarchar(max) NOT NULL,
    [UserId] uniqueidentifier NULL,
    CONSTRAINT [PK_SearchCriterias] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_SearchCriterias_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id])
);
GO

CREATE TABLE [ModerationLogs] (
    [Id] int NOT NULL IDENTITY,
    [PropertyId] uniqueidentifier NOT NULL,
    [ModeratorId] uniqueidentifier NOT NULL,
    [Action] nvarchar(max) NOT NULL,
    [Timestamp] datetimeoffset NOT NULL,
    CONSTRAINT [PK_ModerationLogs] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_ModerationLogs_AspNetUsers_ModeratorId] FOREIGN KEY ([ModeratorId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_ModerationLogs_Properties_PropertyId] FOREIGN KEY ([PropertyId]) REFERENCES [Properties] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [Photos] (
    [Id] int NOT NULL IDENTITY,
    [PropertyId] uniqueidentifier NOT NULL,
    [Url] nvarchar(max) NOT NULL,
    [UploadedAt] datetimeoffset NOT NULL,
    CONSTRAINT [PK_Photos] PRIMARY KEY ([PropertyId], [Id]),
    CONSTRAINT [FK_Photos_Properties_PropertyId] FOREIGN KEY ([PropertyId]) REFERENCES [Properties] ([Id]) ON DELETE CASCADE
);
GO

CREATE INDEX [IX_AspNetRoleClaims_RoleId] ON [AspNetRoleClaims] ([RoleId]);
GO

CREATE UNIQUE INDEX [RoleNameIndex] ON [AspNetRoles] ([NormalizedName]) WHERE [NormalizedName] IS NOT NULL;
GO

CREATE INDEX [IX_AspNetUserClaims_UserId] ON [AspNetUserClaims] ([UserId]);
GO

CREATE INDEX [IX_AspNetUserLogins_UserId] ON [AspNetUserLogins] ([UserId]);
GO

CREATE INDEX [IX_AspNetUserRoles_RoleId] ON [AspNetUserRoles] ([RoleId]);
GO

CREATE INDEX [EmailIndex] ON [AspNetUsers] ([NormalizedEmail]);
GO

CREATE INDEX [IX_AspNetUsers_AgencyId] ON [AspNetUsers] ([AgencyId]);
GO

CREATE UNIQUE INDEX [UserNameIndex] ON [AspNetUsers] ([NormalizedUserName]) WHERE [NormalizedUserName] IS NOT NULL;
GO

CREATE INDEX [IX_ModerationLogs_ModeratorId] ON [ModerationLogs] ([ModeratorId]);
GO

CREATE INDEX [IX_ModerationLogs_PropertyId] ON [ModerationLogs] ([PropertyId]);
GO

CREATE INDEX [IX_Notifications_IsRead] ON [Notifications] ([IsRead]);
GO

CREATE INDEX [IX_Notifications_UserId] ON [Notifications] ([UserId]);
GO

CREATE INDEX [IX_Properties_AgencyId] ON [Properties] ([AgencyId]);
GO

CREATE INDEX [IX_Properties_Price] ON [Properties] ([Price]);
GO

CREATE INDEX [IX_Properties_Status] ON [Properties] ([Status]);
GO

CREATE INDEX [IX_Properties_UserId] ON [Properties] ([UserId]);
GO

CREATE INDEX [IX_SearchCriterias_UserId] ON [SearchCriterias] ([UserId]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250612175622_initialmigration', N'8.0.16');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [SearchCriterias] DROP CONSTRAINT [FK_SearchCriterias_AspNetUsers_UserId];
GO

DROP INDEX [IX_SearchCriterias_UserId] ON [SearchCriterias];
DECLARE @var0 sysname;
SELECT @var0 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[SearchCriterias]') AND [c].[name] = N'UserId');
IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [SearchCriterias] DROP CONSTRAINT [' + @var0 + '];');
UPDATE [SearchCriterias] SET [UserId] = '00000000-0000-0000-0000-000000000000' WHERE [UserId] IS NULL;
ALTER TABLE [SearchCriterias] ALTER COLUMN [UserId] uniqueidentifier NOT NULL;
ALTER TABLE [SearchCriterias] ADD DEFAULT '00000000-0000-0000-0000-000000000000' FOR [UserId];
CREATE INDEX [IX_SearchCriterias_UserId] ON [SearchCriterias] ([UserId]);
GO

ALTER TABLE [Properties] ADD [Bedrooms] int NOT NULL DEFAULT 0;
GO

ALTER TABLE [Properties] ADD [SurfaceArea] decimal(18,2) NOT NULL DEFAULT 0.0;
GO

ALTER TABLE [Photos] ADD [IsMain] bit NOT NULL DEFAULT CAST(0 AS bit);
GO

ALTER TABLE [Agencies] ADD [LogoUrl] nvarchar(max) NULL;
GO

ALTER TABLE [SearchCriterias] ADD CONSTRAINT [FK_SearchCriterias_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250613194157_adduserid', N'8.0.16');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [Notifications] DROP CONSTRAINT [FK_Notifications_AspNetUsers_UserId];
GO

DROP INDEX [IX_Notifications_UserId] ON [Notifications];
GO

DECLARE @var1 sysname;
SELECT @var1 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Notifications]') AND [c].[name] = N'UserId');
IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [Notifications] DROP CONSTRAINT [' + @var1 + '];');
ALTER TABLE [Notifications] DROP COLUMN [UserId];
GO

DECLARE @var2 sysname;
SELECT @var2 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Notifications]') AND [c].[name] = N'SentAt');
IF @var2 IS NOT NULL EXEC(N'ALTER TABLE [Notifications] DROP CONSTRAINT [' + @var2 + '];');
ALTER TABLE [Notifications] ALTER COLUMN [SentAt] datetimeoffset NULL;
GO

ALTER TABLE [Notifications] ADD [AgencyId] int NULL;
GO

ALTER TABLE [Notifications] ADD [RecipientEmail] nvarchar(max) NOT NULL DEFAULT N'';
GO

ALTER TABLE [Notifications] ADD [RecipientId] uniqueidentifier NULL;
GO

ALTER TABLE [Notifications] ADD [SenderEmail] nvarchar(max) NOT NULL DEFAULT N'';
GO

CREATE INDEX [IX_Notifications_AgencyId] ON [Notifications] ([AgencyId]);
GO

CREATE INDEX [IX_Notifications_RecipientId] ON [Notifications] ([RecipientId]);
GO

CREATE INDEX [IX_Notifications_SentAt] ON [Notifications] ([SentAt]);
GO

ALTER TABLE [Notifications] ADD CONSTRAINT [FK_Notifications_Agencies_AgencyId] FOREIGN KEY ([AgencyId]) REFERENCES [Agencies] ([Id]);
GO

ALTER TABLE [Notifications] ADD CONSTRAINT [FK_Notifications_AspNetUsers_RecipientId] FOREIGN KEY ([RecipientId]) REFERENCES [AspNetUsers] ([Id]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250613230212_updateNotificationEntity', N'8.0.16');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

DECLARE @var3 sysname;
SELECT @var3 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Properties]') AND [c].[name] = N'SurfaceArea');
IF @var3 IS NOT NULL EXEC(N'ALTER TABLE [Properties] DROP CONSTRAINT [' + @var3 + '];');
ALTER TABLE [Properties] ALTER COLUMN [SurfaceArea] int NOT NULL;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250613231714_updatePropertyEntity', N'8.0.16');
GO

COMMIT;
GO

