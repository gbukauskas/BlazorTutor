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

