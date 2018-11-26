USE [LiftInstallationDataDB]
GO

INSERT INTO [dbo].[SensorBoxInfo]
           ([TownCouncil]
           ,[BlockNo]
           ,[LiftNo]
           ,[PostalCode]
           ,[SIMCard]
           ,[LMPD])
     VALUES
           (<TownCouncil, varchar(30),>
           ,<BlockNo, int,>
           ,<LiftNo, varchar(40),>
           ,<PostalCode, int,>
           ,<SIMCard, varchar(40),>
           ,<LMPD, varchar(40),>)
GO

