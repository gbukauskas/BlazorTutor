ALTER TABLE Categories ADD IsDeleted INTEGER NOT NULL DEFAULT 0;
ALTER TABLE Categories ADD Version INTEGER NOT NULL DEFAULT 0;

ALTER TABLE CustomerDemographics ADD IsDeleted INTEGER NOT NULL DEFAULT 0;
ALTER TABLE CustomerDemographics ADD Version INTEGER NOT NULL DEFAULT 0;

ALTER TABLE Customers ADD IsDeleted INTEGER NOT NULL DEFAULT 0;
ALTER TABLE Customers ADD Version INTEGER NOT NULL DEFAULT 0;

ALTER TABLE Employees ADD IsDeleted INTEGER NOT NULL DEFAULT 0;
ALTER TABLE Employees ADD Version INTEGER NOT NULL DEFAULT 0;

ALTER TABLE [Order Details] ADD IsDeleted INTEGER NOT NULL DEFAULT 0;
ALTER TABLE [Order Details] ADD Version INTEGER NOT NULL DEFAULT 0;

ALTER TABLE Orders ADD IsDeleted INTEGER NOT NULL DEFAULT 0;
ALTER TABLE Orders ADD Version INTEGER NOT NULL DEFAULT 0;

ALTER TABLE Products ADD IsDeleted INTEGER NOT NULL DEFAULT 0;
ALTER TABLE Products ADD Version INTEGER NOT NULL DEFAULT 0;

ALTER TABLE Regions ADD IsDeleted INTEGER NOT NULL DEFAULT 0;
ALTER TABLE Regions ADD Version INTEGER NOT NULL DEFAULT 0;

ALTER TABLE Shippers ADD IsDeleted INTEGER NOT NULL DEFAULT 0;
ALTER TABLE Shippers ADD Version INTEGER NOT NULL DEFAULT 0;

ALTER TABLE Suppliers ADD IsDeleted INTEGER NOT NULL DEFAULT 0;
ALTER TABLE Suppliers ADD Version INTEGER NOT NULL DEFAULT 0;

ALTER TABLE Territories ADD IsDeleted INTEGER NOT NULL DEFAULT 0;
ALTER TABLE Territories ADD Version INTEGER NOT NULL DEFAULT 0;

CREATE TRIGGER IF NOT EXISTS UpdateCategoriesVersion
   AFTER UPDATE ON Categories
   WHEN NEW.Version >= 0
BEGIN
    UPDATE Categories
    SET Version = Version + 1
    WHERE rowid = NEW.rowid; 
END;
CREATE TRIGGER IF NOT EXISTS TestVersionCategories
BEFORE UPDATE ON Categories
WHEN NEW.Version >= 0
BEGIN
	SELECT
	CASE
		WHEN NEW.Version < OLD.Version THEN RAISE (ABORT, 'Concurrency error')
	END;
END;

CREATE TRIGGER IF NOT EXISTS UpdateCustomerDemographicsVersion
   AFTER UPDATE ON CustomerDemographics
   WHEN NEW.Version >= 0
BEGIN
    UPDATE CustomerDemographics
    SET Version = Version + 1
    WHERE rowid = NEW.rowid; 
END;
CREATE TRIGGER IF NOT EXISTS TestVersionDemographicsVersion
BEFORE UPDATE ON CustomerDemographics
WHEN NEW.Version >= 0
BEGIN
	SELECT
	CASE
		WHEN NEW.Version < OLD.Version THEN RAISE (ABORT, 'Concurrency error')
	END;
END;

CREATE TRIGGER IF NOT EXISTS UpdateCustomersVersion
AFTER UPDATE ON Customers
WHEN NEW.Version >= 0
BEGIN
    UPDATE Customers
    SET Version = Version + 1
    WHERE rowid = NEW.rowid; 
END;
CREATE TRIGGER IF NOT EXISTS TestVersionCustomers
BEFORE UPDATE ON Customers
WHEN NEW.Version >= 0
BEGIN
	SELECT
	CASE
		WHEN NEW.Version < OLD.Version THEN RAISE (ABORT, 'Concurrency error')
	END;
END;

CREATE TRIGGER IF NOT EXISTS UpdateEmployeesVersion
AFTER UPDATE ON Employees
WHEN NEW.Version >= 0
BEGIN
    UPDATE Employees
    SET Version = Version + 1
    WHERE rowid = NEW.rowid; 
END;
CREATE TRIGGER IF NOT EXISTS TestVersionEmployees
BEFORE UPDATE ON Employees
WHEN NEW.Version >= 0
BEGIN
	SELECT
	CASE
		WHEN NEW.Version < OLD.Version THEN RAISE (ABORT, 'Concurrency error')
	END;
END;

CREATE TRIGGER IF NOT EXISTS UpdateOrderDetailsVersion
AFTER UPDATE ON [Order Details]
WHEN NEW.Version >= 0
BEGIN
    UPDATE [Order Details]
    SET Version = Version + 1
    WHERE rowid = NEW.rowid; 
END;
CREATE TRIGGER IF NOT EXISTS TestVersionOrderDetails
BEFORE UPDATE ON [Order Details]
WHEN NEW.Version >= 0
BEGIN
	SELECT
	CASE
		WHEN NEW.Version < OLD.Version THEN RAISE (ABORT, 'Concurrency error')
	END;
END;

CREATE TRIGGER IF NOT EXISTS UpdateOrdersVersion
AFTER UPDATE ON Orders
WHEN NEW.Version >= 0
BEGIN
    UPDATE Orders
    SET Version = Version + 1
    WHERE rowid = NEW.rowid; 
END;
CREATE TRIGGER IF NOT EXISTS TestVersionOrders
BEFORE UPDATE ON Orders
WHEN NEW.Version >= 0
BEGIN
	SELECT
	CASE
		WHEN NEW.Version < OLD.Version THEN RAISE (ABORT, 'Concurrency error')
	END;
END;

CREATE TRIGGER IF NOT EXISTS UpdateProductsVersion
AFTER UPDATE ON Products
WHEN NEW.Version >= 0
BEGIN
    UPDATE Products
    SET Version = Version + 1
    WHERE rowid = NEW.rowid; 
END;
CREATE TRIGGER IF NOT EXISTS TestVersionProducts
BEFORE UPDATE ON Products
WHEN NEW.Version >= 0
BEGIN
	SELECT
	CASE
		WHEN NEW.Version < OLD.Version THEN RAISE (ABORT, 'Concurrency error')
	END;
END;

CREATE TRIGGER IF NOT EXISTS UpdateRegionsVersion
AFTER UPDATE ON Regions
WHEN NEW.Version >= 0
BEGIN
    UPDATE Regions
    SET Version = Version + 1
    WHERE rowid = NEW.rowid; 
END;
CREATE TRIGGER IF NOT EXISTS TestVersionRegions
BEFORE UPDATE ON Regions
WHEN NEW.Version >= 0
BEGIN
	SELECT
	CASE
		WHEN NEW.Version < OLD.Version THEN RAISE (ABORT, 'Concurrency error')
	END;
END;

CREATE TRIGGER IF NOT EXISTS UpdateShippersVersion
AFTER UPDATE ON Shippers
WHEN NEW.Version >= 0
BEGIN
    UPDATE Shippers
    SET Version = Version + 1
    WHERE rowid = NEW.rowid; 
END;
CREATE TRIGGER IF NOT EXISTS TestVersionShippers
BEFORE UPDATE ON Shippers
WHEN NEW.Version >= 0
BEGIN
	SELECT
	CASE
		WHEN NEW.Version < OLD.Version THEN RAISE (ABORT, 'Concurrency error')
	END;
END;

CREATE TRIGGER IF NOT EXISTS UpdateSuppliersVersion
AFTER UPDATE ON Suppliers
WHEN NEW.Version >= 0
BEGIN
    UPDATE Suppliers
    SET Version = Version + 1
    WHERE rowid = NEW.rowid; 
END;
CREATE TRIGGER IF NOT EXISTS TestVersionSuppliers
BEFORE UPDATE ON Suppliers
WHEN NEW.Version >= 0
BEGIN
	SELECT
	CASE
		WHEN NEW.Version < OLD.Version THEN RAISE (ABORT, 'Concurrency error')
	END;
END;

CREATE TRIGGER IF NOT EXISTS UpdateTerritoriesVersion
AFTER UPDATE ON Territories
WHEN NEW.Version >= 0
BEGIN
    UPDATE Territories
    SET Version = Version + 1
    WHERE rowid = NEW.rowid; 
END;
CREATE TRIGGER IF NOT EXISTS TestVersionTerritories
BEFORE UPDATE ON Territories
WHEN NEW.Version >= 0
BEGIN
	SELECT
	CASE
		WHEN NEW.Version < OLD.Version THEN RAISE (ABORT, 'Concurrency error')
	END;
END;

CREATE TABLE IF NOT EXISTS AppStates(
	AppName TEXT NOT NULL,
	UserId TEXT NOT NULL,
	ComponentName TEXT NOT NULL,
	SortState TEXT,
	FilterFieldName TEXT,
	FilterFieldValue TEXT,
	FilterIsDateValue INTEGER,
	PagerButtonCount INTEGER,
	PagerRowCount INTEGER,
	PagerPageCount INTEGER,
	PagerPageNumber INTEGER,
	PagerPageSize INTEGER,
	PagerBaseUrl TEXT,
	IsDeleted INTEGER NOT NULL DEFAULT 0,
	DateCreated DATE DEFAULT CURRENT_DATE,
	PRIMARY KEY ( AppName, UserId, ComponentName)
);
