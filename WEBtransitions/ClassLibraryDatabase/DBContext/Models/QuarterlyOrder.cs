using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace WEBtransitions.ClassLibraryDatabase.DBContext;

/// <summary>
/// SELECT DISTINCT Customers.CustomerID, Customers.CompanyName, Customers.City, Customers.Country
///     FROM Customers RIGHT JOIN Orders ON Customers.CustomerID = Orders.CustomerID
/// WHERE Orders.OrderDate BETWEEN '19970101' And '19971231'

/// </summary>
public partial class QuarterlyOrder
{
    public required string CustomerId { get; set; }

    public required string CompanyName { get; set; }

    public string? City { get; set; }

    public string? Country { get; set; }

    internal static void Configure(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<QuarterlyOrder>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Quarterly Orders");

            entity.Property(e => e.CustomerId).HasColumnName("CustomerID").IsRequired().HasMaxLength(5);
            entity.Property(e => e.CompanyName).IsRequired().HasMaxLength(40);
            entity.Property(e => e.City).HasMaxLength(15);
            entity.Property(e => e.Country).HasMaxLength(15);
        });
    }
}
