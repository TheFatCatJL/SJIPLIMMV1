
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 01/17/2019 14:08:01
-- Generated from EDMX file: C:\Users\601733\source\repos\SJIPLIMMV1\SJIP-LIMMV1\Models\BoxEntity.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [BoxModel];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_CommissionRecordComBoxInfo]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ComBoxInfoes] DROP CONSTRAINT [FK_CommissionRecordComBoxInfo];
GO
IF OBJECT_ID(N'[dbo].[FK_AddressComBoxInfo]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ComBoxInfoes] DROP CONSTRAINT [FK_AddressComBoxInfo];
GO
IF OBJECT_ID(N'[dbo].[FK_AddressPreBoxInfo]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PreBoxInfoes] DROP CONSTRAINT [FK_AddressPreBoxInfo];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Addresses]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Addresses];
GO
IF OBJECT_ID(N'[dbo].[ComBoxInfoes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ComBoxInfoes];
GO
IF OBJECT_ID(N'[dbo].[CommissionRecords]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CommissionRecords];
GO
IF OBJECT_ID(N'[dbo].[PreBoxInfoes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PreBoxInfoes];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Addresses'
CREATE TABLE [dbo].[Addresses] (
    [postalcode] int  NOT NULL,
    [blk] nvarchar(max)  NULL,
    [road] nvarchar(max)  NULL,
    [legacycode1] nvarchar(max)  NULL,
    [legacycode2] nvarchar(max)  NULL,
    [legacycode3] nvarchar(max)  NULL
);
GO

-- Creating table 'ComBoxInfoes'
CREATE TABLE [dbo].[ComBoxInfoes] (
    [comboxId] int IDENTITY(1,1) NOT NULL,
    [rptlift] nvarchar(max)  NOT NULL,
    [rptcomment] nvarchar(max)  NULL,
    [teamname] nvarchar(max)  NULL,
    [techname] nvarchar(max)  NULL,
    [status] nvarchar(max)  NULL,
    [history] nvarchar(max)  NULL,
    [postalcode] int  NULL,
    [comrecId] int  NULL,
    [lmpdnum] nvarchar(max)  NOT NULL,
    [rptdate] datetime  NOT NULL,
    [ismatched] bit  NOT NULL
);
GO

-- Creating table 'CommissionRecords'
CREATE TABLE [dbo].[CommissionRecords] (
    [comrecId] int IDENTITY(1,1) NOT NULL,
    [supname] nvarchar(max)  NOT NULL,
    [comrecorddate] datetime  NOT NULL,
    [history] nvarchar(max)  NULL,
    [status] nvarchar(max)  NULL,
    [comment] nvarchar(max)  NULL
);
GO

-- Creating table 'PreBoxInfoes'
CREATE TABLE [dbo].[PreBoxInfoes] (
    [preboxId] int IDENTITY(1,1) NOT NULL,
    [lmpdnum] nvarchar(max)  NOT NULL,
    [jsonid] nvarchar(max)  NOT NULL,
    [checkername] nvarchar(max)  NOT NULL,
    [checkdate] datetime  NULL,
    [status] nvarchar(max)  NULL,
    [history] nvarchar(max)  NULL,
    [isdeployed] bit  NULL,
    [installdate] datetime  NULL,
    [lift] nvarchar(max)  NULL,
    [simnum] nvarchar(max)  NOT NULL,
    [telco] nvarchar(max)  NOT NULL,
    [postalcode] int  NULL,
    [ismatched] bit  NOT NULL
);
GO

-- Creating table 'ContactForms'
CREATE TABLE [dbo].[ContactForms] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Email] nvarchar(max)  NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [Message] nvarchar(max)  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [postalcode] in table 'Addresses'
ALTER TABLE [dbo].[Addresses]
ADD CONSTRAINT [PK_Addresses]
    PRIMARY KEY CLUSTERED ([postalcode] ASC);
GO

-- Creating primary key on [comboxId] in table 'ComBoxInfoes'
ALTER TABLE [dbo].[ComBoxInfoes]
ADD CONSTRAINT [PK_ComBoxInfoes]
    PRIMARY KEY CLUSTERED ([comboxId] ASC);
GO

-- Creating primary key on [comrecId] in table 'CommissionRecords'
ALTER TABLE [dbo].[CommissionRecords]
ADD CONSTRAINT [PK_CommissionRecords]
    PRIMARY KEY CLUSTERED ([comrecId] ASC);
GO

-- Creating primary key on [preboxId] in table 'PreBoxInfoes'
ALTER TABLE [dbo].[PreBoxInfoes]
ADD CONSTRAINT [PK_PreBoxInfoes]
    PRIMARY KEY CLUSTERED ([preboxId] ASC);
GO

-- Creating primary key on [Id] in table 'ContactForms'
ALTER TABLE [dbo].[ContactForms]
ADD CONSTRAINT [PK_ContactForms]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [comrecId] in table 'ComBoxInfoes'
ALTER TABLE [dbo].[ComBoxInfoes]
ADD CONSTRAINT [FK_CommissionRecordComBoxInfo]
    FOREIGN KEY ([comrecId])
    REFERENCES [dbo].[CommissionRecords]
        ([comrecId])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CommissionRecordComBoxInfo'
CREATE INDEX [IX_FK_CommissionRecordComBoxInfo]
ON [dbo].[ComBoxInfoes]
    ([comrecId]);
GO

-- Creating foreign key on [postalcode] in table 'ComBoxInfoes'
ALTER TABLE [dbo].[ComBoxInfoes]
ADD CONSTRAINT [FK_AddressComBoxInfo]
    FOREIGN KEY ([postalcode])
    REFERENCES [dbo].[Addresses]
        ([postalcode])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AddressComBoxInfo'
CREATE INDEX [IX_FK_AddressComBoxInfo]
ON [dbo].[ComBoxInfoes]
    ([postalcode]);
GO

-- Creating foreign key on [postalcode] in table 'PreBoxInfoes'
ALTER TABLE [dbo].[PreBoxInfoes]
ADD CONSTRAINT [FK_AddressPreBoxInfo]
    FOREIGN KEY ([postalcode])
    REFERENCES [dbo].[Addresses]
        ([postalcode])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AddressPreBoxInfo'
CREATE INDEX [IX_FK_AddressPreBoxInfo]
ON [dbo].[PreBoxInfoes]
    ([postalcode]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------