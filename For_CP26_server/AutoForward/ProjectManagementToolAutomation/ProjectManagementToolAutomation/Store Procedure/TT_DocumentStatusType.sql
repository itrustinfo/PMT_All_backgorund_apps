
/****** Object:  UserDefinedTableType [dbo].[TT_DocumentStatusType]    Script Date: 07/02/2022 15:43:44 ******/
CREATE TYPE [dbo].[TT_DocumentStatusType] AS TABLE(
	[StatusUID] [uniqueidentifier] NULL,
	[DocumentUID] [uniqueidentifier] NULL,
	[ActivityType] [varchar](50) NULL,
	[ActivityDate] [date] NULL,
	[DocumentDate] [date] NULL,
	[Status_Comments] [varchar](750) NULL
)
GO


