using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace WEBtransitions.ClassLibraryDatabase.DBContext;

/// <summary>
/// CREATE VIEW [Invoices] 
/// AS
/// SELECT Orders.ShipName,
///       Orders.ShipAddress,
///       Orders.ShipCity,
///       Orders.ShipRegion, 
///       Orders.ShipPostalCode,
///       Orders.ShipCountry,
///       Orders.CustomerID,
///       Customers.CompanyName AS CustomerName, 
///       Customers.Address,
///       Customers.City,
///       Customers.Region,
///       Customers.PostalCode,
///       Customers.Country,
///       (Employees.FirstName + ' ' + Employees.LastName) AS Salesperson,
///       Orders.OrderID,
///       Orders.OrderDate,
///       Orders.RequiredDate,
///       Orders.ShippedDate, 
///       Shippers.CompanyName As ShipperName,
///       [Order Details].ProductID,
///       Products.ProductName, 
///       [Order Details].UnitPrice,
///       [Order Details].Quantity,
///       [Order Details].Discount, 
///       ((([Order Details].UnitPrice* Quantity* (1-Discount))/100)*100) AS ExtendedPrice,
///       Orders.Freight
/// FROM Customers
///  JOIN Orders ON Customers.CustomerID = Orders.CustomerID
///    JOIN Employees ON Employees.EmployeeID = Orders.EmployeeID
///     JOIN[Order Details] ON Orders.OrderID = [Order Details].OrderID
///      JOIN Products ON Products.ProductID = [Order Details].ProductID
///       JOIN Shippers ON Shippers.ShipperID = Orders.ShipVia
/// </summary>
public partial class Invoice
{
    public string? ShipName { get; set; }

    public string? ShipAddress { get; set; }

    public string? ShipCity { get; set; }

    public string? ShipRegion { get; set; }

    public string? ShipPostalCode { get; set; }

    public string? ShipCountry { get; set; }

    public string? CustomerId { get; set; }

    public required string CustomerName { get; set; }

    public string? Address { get; set; }

    public string? City { get; set; }

    public string? Region { get; set; }

    public string? PostalCode { get; set; }

    public string? Country { get; set; }

    public string? Salesperson { get; set; }

    public int OrderId { get; set; }

    public DateTime? OrderDate { get; set; }

    public DateTime? RequiredDate { get; set; }

    public DateTime? ShippedDate { get; set; }

    public required string ShipperName { get; set; }

    public int ProductId { get; set; }

    public required string ProductName { get; set; }

    public double UnitPrice { get; set; }

    public int Quantity { get; set; }

    public double Discount { get; set; }

    public double? ExtendedPrice { get; set; }

    public double? Freight { get; set; }

    internal static void Configure(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Invoice>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Invoices");

            entity.Property(e => e.ShipName).HasMaxLength(40).HasColumnType("TEXT");
            entity.Property(e => e.ShipAddress).HasMaxLength(60).HasColumnType("TEXT");
            entity.Property(e => e.ShipCity).HasMaxLength(15).HasColumnType("TEXT");
            entity.Property(e => e.ShipRegion).HasMaxLength(15).HasColumnType("TEXT");
            entity.Property(e => e.ShipPostalCode).HasMaxLength(10).HasColumnType("TEXT");
            entity.Property(e => e.ShipCountry).HasMaxLength(15).HasColumnType("TEXT");
            entity.Property(e => e.CustomerId).HasColumnName("CustomerID").HasMaxLength(5).HasColumnType("TEXT");
            entity.Property(e => e.CustomerName).HasMaxLength(40).HasColumnType("TEXT");
            entity.Property(e => e.Address).HasMaxLength(60).HasColumnType("TEXT");
            entity.Property(e => e.City).HasMaxLength(15).HasColumnType("TEXT");
            entity.Property(e => e.Region).HasMaxLength(15).HasColumnType("TEXT");
            entity.Property(e => e.PostalCode).HasMaxLength(10).HasColumnType("TEXT");
            entity.Property(e => e.Country).HasMaxLength(15).HasColumnType("TEXT");
            entity.Property(e => e.Salesperson).HasMaxLength(31).HasColumnType("TEXT");
            entity.Property(e => e.OrderId).HasColumnName("OrderID");
            entity.Property(e => e.OrderDate).HasColumnType("DATETIME");
            entity.Property(e => e.RequiredDate).HasColumnType("DATETIME");
            entity.Property(e => e.ShippedDate).HasColumnType("DATETIME");
            entity.Property(e => e.ShipperName).HasMaxLength(40).HasColumnType("TEXT");
            entity.Property(e => e.ProductId).HasColumnName("ProductID").HasColumnType("INTEGER");
            entity.Property(e => e.ProductName).HasMaxLength(40).HasColumnType("TEXT");
            entity.Property(e => e.UnitPrice).HasColumnType("NUMERIC");
            entity.Property(e => e.Quantity).HasColumnType("INTEGER");
            entity.Property(e => e.Discount).HasColumnType("REAL");
            entity.Property(e => e.ExtendedPrice).HasColumnType("REAL");
            entity.Property(e => e.Freight).HasColumnType("NUMERIC");
        });
    }
}
