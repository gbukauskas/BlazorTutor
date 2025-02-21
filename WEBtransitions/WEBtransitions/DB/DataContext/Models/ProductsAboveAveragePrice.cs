using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace WEBtransitions.DB.DataContext.Models;

/// <summary>
/// SELECT Products.ProductName, Products.UnitPrice
/// FROM Products
///     WHERE Products.UnitPrice>(SELECT AVG(UnitPrice) From Products)
///     --ORDER BY Products.UnitPrice DESC
/// </summary>
public partial class ProductsAboveAveragePrice
{
    public required string ProductName { get; set; }

    public double? UnitPrice { get; set; }

    internal static void Configure(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ProductsAboveAveragePrice>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Products Above Average Price");

            entity.Property(e => e.ProductName).HasColumnType("TEXT").HasMaxLength(40);
            entity.Property(e => e.UnitPrice).HasColumnType("REAL");
        });
    }
}
