using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace WEBtransitions.DB.DataContext.Models;

/// <summary>
///     CREATE VIEW [Category Sales for 1997] AS
///         SELECT [Product Sales for 1997].CategoryName, Sum([Product Sales for 1997].ProductSales) AS CategorySales
///         FROM[Product Sales for 1997]
///         GROUP BY[Product Sales for 1997].CategoryName
/// </summary>
public partial class CategorySalesFor1997
{
    public required string CategoryName { get; set; }

    public decimal? CategorySales { get; set; }

    static internal void Configure(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CategorySalesFor1997>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Category Sales for 1997");

            entity.Property(e => e.CategoryName)
                .IsRequired()
                .HasMaxLength(15)
                .HasColumnType("TEXT");
            entity.Property(e => e.CategorySales).HasColumnType("REAL");
        });
    }
}
