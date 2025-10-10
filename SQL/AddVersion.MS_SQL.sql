IF NOT EXISTS(SELECT 1 FROM sys.columns 
          WHERE Name = N'Version'
          AND Object_ID = Object_ID(N'dbo.Categories'))
BEGIN
    ALTER TABLE dbo.Categories ADD IsDeleted BIT NOT NULL DEFAULT 0;
	ALTER TABLE dbo.Categories ADD Version rowversion;
END
IF NOT EXISTS(SELECT 1 FROM sys.columns 
          WHERE Name = N'Version'
          AND Object_ID = Object_ID(N'dbo.CustomerDemographics'))
BEGIN
    ALTER TABLE dbo.CustomerDemographics ADD IsDeleted BIT NOT NULL DEFAULT 0;
	ALTER TABLE dbo.CustomerDemographics ADD Version rowversion;
END
IF NOT EXISTS(SELECT 1 FROM sys.columns 
          WHERE Name = N'Version'
          AND Object_ID = Object_ID(N'dbo.Customers'))
BEGIN
    ALTER TABLE dbo.Customers ADD IsDeleted BIT NOT NULL DEFAULT 0;
	ALTER TABLE dbo.Customers ADD Version rowversion;
END
IF NOT EXISTS(SELECT 1 FROM sys.columns 
          WHERE Name = N'Version'
          AND Object_ID = Object_ID(N'dbo.Employees'))
BEGIN
    ALTER TABLE dbo.Employees ADD IsDeleted BIT NOT NULL DEFAULT 0;
	ALTER TABLE dbo.Employees ADD Version rowversion;
END
IF NOT EXISTS(SELECT 1 FROM sys.columns 
          WHERE Name = N'Version'
          AND Object_ID = Object_ID(N'dbo.[Order Details]'))
BEGIN
    ALTER TABLE dbo.[Order Details] ADD IsDeleted BIT NOT NULL DEFAULT 0;
	ALTER TABLE dbo.[Order Details] ADD Version rowversion;
END
IF NOT EXISTS(SELECT 1 FROM sys.columns 
          WHERE Name = N'Version'
          AND Object_ID = Object_ID(N'dbo.Orders'))
BEGIN
    ALTER TABLE dbo.Orders ADD IsDeleted BIT NOT NULL DEFAULT 0;
	ALTER TABLE dbo.Orders ADD Version rowversion;
END
IF NOT EXISTS(SELECT 1 FROM sys.columns 
          WHERE Name = N'Version'
          AND Object_ID = Object_ID(N'dbo.Products'))
BEGIN
    ALTER TABLE dbo.Products ADD IsDeleted BIT NOT NULL DEFAULT 0;
	ALTER TABLE dbo.Products ADD Version rowversion;
END
IF NOT EXISTS(SELECT 1 FROM sys.columns 
          WHERE Name = N'Version'
          AND Object_ID = Object_ID(N'dbo.Region'))
BEGIN
    ALTER TABLE dbo.Region ADD IsDeleted BIT NOT NULL DEFAULT 0;
	ALTER TABLE dbo.Region ADD Version rowversion;
END
IF NOT EXISTS(SELECT 1 FROM sys.columns 
          WHERE Name = N'Version'
          AND Object_ID = Object_ID(N'dbo.Shippers'))
BEGIN
    ALTER TABLE dbo.Shippers ADD IsDeleted BIT NOT NULL DEFAULT 0;
	ALTER TABLE dbo.Shippers ADD Version rowversion;
END
IF NOT EXISTS(SELECT 1 FROM sys.columns 
          WHERE Name = N'Version'
          AND Object_ID = Object_ID(N'dbo.Suppliers'))
BEGIN
    ALTER TABLE dbo.Suppliers ADD IsDeleted BIT NOT NULL DEFAULT 0;
	ALTER TABLE dbo.Suppliers ADD Version rowversion;
END
IF NOT EXISTS(SELECT 1 FROM sys.columns 
          WHERE Name = N'Version'
          AND Object_ID = Object_ID(N'dbo.Territories'))
BEGIN
    ALTER TABLE dbo.Territories ADD IsDeleted BIT NOT NULL DEFAULT 0;
	ALTER TABLE dbo.Territories ADD Version rowversion;
END

IF OBJECT_ID(N'dbo.[AppStates', N'U') IS NULL
BEGIN
	CREATE TABLE [dbo].[AppStates](
		[AppName] [nvarchar](100) NOT NULL,
		[UserId] [nvarchar](200) NOT NULL,
		[ComponentName] [varchar](100) NOT NULL,
		[SortState] [nvarchar](2000) NULL,
		[FilterFieldName] [varchar](500) NULL,
		[FilterFieldValue] [nvarchar](2000) NULL,
		[FilterIsDateValue] [bit] NOT NULL,
		[PagerButtonCount] [tinyint] NULL,
		[PagerRowCount] [int] NULL,
		[PagerPageCount] [smallint] NULL,
		[PagerPageNumber] [smallint] NULL,
		[PagerPageSize] [smallint] NULL,
		[PagerBaseUrl] [varchar](200) NULL,
		[IsDeleted] [bit] NOT NULL,
		[DateCreated] [datetime] DEFAULT GETUTCDATE(),
		CONSTRAINT [PK_AppState] PRIMARY KEY CLUSTERED 
	(
		[AppName] ASC,
		[UserId] ASC,
		[ComponentName] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
	) ON [PRIMARY]
END
GO

