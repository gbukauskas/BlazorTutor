using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace WEBtransitions.ClassLibraryDatabase.DBContext;

/// <summary>
/// SELECT Categories.CategoryName, Products.ProductName, Products.QuantityPerUnit, Products.UnitsInStock, Products.Discontinued
/// FROM Categories INNER JOIN Products ON Categories.CategoryID = Products.CategoryID
/// WHERE Products.Discontinued<> 1
///     --ORDER BY Categories.CategoryName, Products.ProductName

/// </summary>
public partial class ProductsByCategory
{
    public required string CategoryName { get; set; }

    public required string ProductName { get; set; }

    public string? QuantityPerUnit { get; set; }

    public short? UnitsInStock { get; set; }

    public required string Discontinued { get; set; }

    internal static void Configure(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ProductsByCategory>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Products by Category");

            entity.Property(e => e.CategoryName).HasColumnType("TEXT").HasMaxLength(15);
            entity.Property(e => e.ProductName).HasColumnType("TEXT").HasMaxLength(40);
            entity.Property(e => e.QuantityPerUnit).HasColumnType("TEXT").HasMaxLength(20);
            entity.Property(e => e.UnitsInStock).HasColumnType("INTEGER");
            entity.Property(e => e.Discontinued).HasDefaultValue("0").HasColumnType("TEXT").HasMaxLength(1);
        });
    }
}
