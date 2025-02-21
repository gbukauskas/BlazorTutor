using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace WEBtransitions.DB.DataContext.Models;

/// <summary>
///     CREATE VIEW [Customer and Suppliers by City] 
///     AS
///         SELECT City, CompanyName, ContactName, 'Customers' AS Relationship
///         FROM Customers
///     UNION
///         SELECT City, CompanyName, ContactName, 'Suppliers'
///         FROM Suppliers
///     ORDER BY City, CompanyName
/// </summary>
public partial class CustomerAndSuppliersByCity
{
    public string? City { get; set; }

    public required string CompanyName { get; set; }

    public string? ContactName { get; set; }

    public required string Relationship { get; set; }

    internal static void Configure(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CustomerAndSuppliersByCity>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Customer and Suppliers by City");

            entity.Property(e => e.City).HasMaxLength(15);
            entity.Property(e => e.CompanyName).HasMaxLength(40);
            entity.Property(e =>e.ContactName).HasMaxLength(30);
            entity.Property(e =>e.Relationship).HasMaxLength(9);
        });
    }
}
