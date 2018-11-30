USE [LiftInstallationDataDB]
GO

/****** Object:  Table [dbo].[SensorBoxInfo]    Script Date: 26/11/2018 8:48:16 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SensorBoxInfo](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[TownCouncil] [varchar](30) NOT NULL,
	[BlockNo] [int] NOT NULL,
	[LiftNo] [varchar](40) NULL,
	[PostalCode] [int] NULL,
	[SIMCard] [varchar](40) NULL,
	[LMPD] [varchar](40) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

