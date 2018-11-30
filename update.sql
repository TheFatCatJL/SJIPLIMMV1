USE [LiftInstallationDataDB]
GO

UPDATE [dbo].[SensorBoxInfo]
   SET [TownCouncil] = <TownCouncil, varchar(30),>
      ,[BlockNo] = <BlockNo, int,>
      ,[LiftNo] = <LiftNo, varchar(40),>
      ,[PostalCode] = <PostalCode, int,>
      ,[SIMCard] = <SIMCard, varchar(40),>
      ,[LMPD] = <LMPD, varchar(40),>
 WHERE <Search Conditions,,>
GO

