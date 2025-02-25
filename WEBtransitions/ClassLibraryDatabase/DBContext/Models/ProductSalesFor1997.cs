using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace WEBtransitions.ClassLibraryDatabase.DBContext;

/// <summary>
/// SELECT Categories.CategoryName, Products.ProductName, 
///     Sum(CONVERT(money, ("Order Details".UnitPrice* Quantity*(1-Discount)/100))*100) AS ProductSales
/// FROM(Categories INNER JOIN Products ON Categories.CategoryID = Products.CategoryID)
/// INNER JOIN(Orders
///        INNER JOIN "Order Details" ON Orders.OrderID = "Order Details".OrderID)
///    ON Products.ProductID = "Order Details".ProductID
/// WHERE (((Orders.ShippedDate) Between '19970101' And '19971231'))
/// GROUP BY Categories.CategoryName, Products.ProductName
/// </summary>
public partial class ProductSalesFor1997
{
    public required string CategoryName { get; set; }

    public required string ProductName { get; set; }

    public Decimal? ProductSales { get; set; }

    internal static void Configure(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ProductSalesFor1997>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Product Sales for 1997");

            entity.Property(e => e.CategoryName).HasColumnType("TEXT").HasMaxLength(15);
            entity.Property(e => e.ProductName).HasColumnType("TEXT").HasMaxLength(40);
            entity.Property(e => e.ProductSales).HasColumnType("REAL");
        });
    }
}
