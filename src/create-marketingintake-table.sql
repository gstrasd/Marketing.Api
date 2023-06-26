﻿USE [OrbitTest]
GO

/****** Object:  Table [dbo].[MarketingIntake]    Script Date: 3/30/2023 10:16:19 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[MarketingIntake](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[OrderId] [int] NULL,
	[LeadSource] [varchar](20) NOT NULL,
	[Payload] [nvarchar](max) NOT NULL,
	[PayloadHash] [uniqueidentifier] NOT NULL,
	[Success] [bit] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[SessionId] [int] NOT NULL,
	[Error] [nvarchar](max) NULL,
 CONSTRAINT [PK_MarketingIntake] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 85) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_MarketingIntake_CreatedDate] ON [dbo].[MarketingIntake]
(
	[CreatedDate] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 85) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_MarketingIntake_OrderId] ON [dbo].[MarketingIntake]
(
	[OrderId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 85) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_MarketingIntake_PayloadHash] ON [dbo].[MarketingIntake]
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 85) ON [PRIMARY]
GO



