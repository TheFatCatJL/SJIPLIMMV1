-- SQL Database Creation for LIMM v0.01B	--
-- Created by SJ-Intern team-Jason Lim		--
USE MASTER;
IF EXISTS (SELECT [name] FROM sys.databases WHERE [name] = 'LiftInstallationDataDB')
BEGIN
ALTER DATABASE LiftInstallationDataDB SET SINGLE_USER WITH ROLLBACK IMMEDIATE
DROP DATABASE LiftInstallationDataDB;
END

CREATE DATABASE LiftInstallationDataDB
GO

USE LiftInstallationDataDB
