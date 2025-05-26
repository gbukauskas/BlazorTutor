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
   AFTER UPDATE 
   ON Categories
BEGIN
    UPDATE Categories
    SET Version = Version + 1
    WHERE rowid = NEW.rowid; 
END;

CREATE TRIGGER IF NOT EXISTS UpdateCustomerDemographicsVersion
   AFTER UPDATE 
   ON CustomerDemographics
BEGIN
    UPDATE CustomerDemographics
    SET Version = Version + 1
    WHERE rowid = NEW.rowid; 
END;

CREATE TRIGGER IF NOT EXISTS UpdateCustomersVersion
   AFTER UPDATE 
   ON Customers
BEGIN
    UPDATE Customers
    SET Version = Version + 1
    WHERE rowid = NEW.rowid; 
END;

CREATE TRIGGER IF NOT EXISTS UpdateEmployeesVersion
   AFTER UPDATE 
   ON Employees
BEGIN
    UPDATE Employees
    SET Version = Version + 1
    WHERE rowid = NEW.rowid; 
END;

CREATE TRIGGER IF NOT EXISTS UpdateOrderDetailsVersion
   AFTER UPDATE 
   ON [Order Details]
BEGIN
    UPDATE [Order Details]
    SET Version = Version + 1
    WHERE rowid = NEW.rowid; 
END;

CREATE TRIGGER IF NOT EXISTS UpdateOrdersVersion
   AFTER UPDATE 
   ON Orders
BEGIN
    UPDATE Orders
    SET Version = Version + 1
    WHERE rowid = NEW.rowid; 
END;

CREATE TRIGGER IF NOT EXISTS UpdateProductsVersion
   AFTER UPDATE 
   ON Products
BEGIN
    UPDATE Products
    SET Version = Version + 1
    WHERE rowid = NEW.rowid; 
END;

CREATE TRIGGER IF NOT EXISTS UpdateRegionsVersion
   AFTER UPDATE 
   ON Regions
BEGIN
    UPDATE Regions
    SET Version = Version + 1
    WHERE rowid = NEW.rowid; 
END;

CREATE TRIGGER IF NOT EXISTS UpdateShippersVersion
   AFTER UPDATE 
   ON Shippers
BEGIN
    UPDATE Shippers
    SET Version = Version + 1
    WHERE rowid = NEW.rowid; 
END;

CREATE TRIGGER IF NOT EXISTS UpdateSuppliersVersion
   AFTER UPDATE 
   ON Suppliers
BEGIN
    UPDATE Suppliers
    SET Version = Version + 1
    WHERE rowid = NEW.rowid; 
END;

CREATE TRIGGER IF NOT EXISTS UpdateTerritoriesVersion
   AFTER UPDATE 
   ON Territories
BEGIN
    UPDATE Territories
    SET Version = Version + 1
    WHERE rowid = NEW.rowid; 
END;
