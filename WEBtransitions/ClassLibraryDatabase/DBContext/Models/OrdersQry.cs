using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace WEBtransitions.ClassLibraryDatabase.DBContext;

/// <summary>
/// SELECT Orders.OrderID, Orders.CustomerID, Orders.EmployeeID, Orders.OrderDate, Orders.RequiredDate,
///    Orders.ShippedDate, Orders.ShipVia, Orders.Freight, Orders.ShipName, Orders.ShipAddress, Orders.ShipCity,
///    Orders.ShipRegion, Orders.ShipPostalCode, Orders.ShipCountry,
///    Customers.CompanyName, Customers.Address, Customers.City, Customers.Region, Customers.PostalCode, Customers.Country
/// FROM Customers INNER JOIN Orders ON Customers.CustomerID = Orders.CustomerID
/// </summary>
public partial class OrdersQry
{
    public int OrderId { get; set; }
    public string? CustomerId { get; set; }
    public int? EmployeeId { get; set; }
    public DateTime? OrderDate { get; set; }
    public DateTime? RequiredDate { get; set; }
    public DateTime? ShippedDate { get; set; }
    public int? ShipVia { get; set; }
    public int? Freight { get; set; }
    public string? ShipName { get; set; }
    public string? ShipAddress { get; set; }
    public string? ShipCity { get; set; }
    public string? ShipRegion { get; set; }
    public string? ShipPostalCode { get; set; }
    public string? ShipCountry { get; set; }
    public required string CompanyName { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? Region { get; set; }
    public string? PostalCode { get; set; }
    public string? Country { get; set; }

    internal static void Configure(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<OrdersQry>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Orders Qry");

            entity.Property(e => e.OrderId).HasColumnName("OrderID").HasColumnType("INTEGER");
            entity.Property(e => e.CustomerId).HasColumnName("CustomerID").HasColumnType("TEXT").HasMaxLength(5);
            entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID").HasColumnType("INTEGER");
            entity.Property(e => e.OrderDate).HasColumnType("DATETIME");
            entity.Property(e => e.RequiredDate).HasColumnType("DATETIME");
            entity.Property(e => e.ShippedDate).HasColumnType("DATETIME");
            entity.Property(e => e.ShipVia).HasColumnType("INTEGER");
            entity.Property(e => e.Freight).HasColumnType("NUMERIC");
            entity.Property(e => e.ShipName).HasColumnType("TEXT").HasMaxLength(40);
            entity.Property(e => e.ShipAddress).HasColumnType("TEXT").HasMaxLength(60);
            entity.Property(e => e.ShipCity).HasColumnType("TEXT").HasMaxLength(15);
            entity.Property(e => e.ShipRegion).HasColumnType("TEXT").HasMaxLength(15);
            entity.Property(e => e.ShipPostalCode).HasColumnType("TEXT").HasMaxLength(10);
            entity.Property(e => e.ShipCountry).HasColumnType("TEXT").HasMaxLength(15);
            entity.Property(e => e.CompanyName).HasColumnType("TEXT").HasMaxLength(40);
            entity.Property(e => e.Address).HasColumnType("TEXT").HasMaxLength(60);
            entity.Property(e => e.City).HasColumnType("TEXT").HasMaxLength(15);
            entity.Property(e => e.Region).HasColumnType("TEXT").HasMaxLength(15);
            entity.Property(e => e.PostalCode).HasColumnType("TEXT").HasMaxLength(10);
            entity.Property(e => e.Country).HasColumnType("TEXT").HasMaxLength(15);
        });
    }
}
