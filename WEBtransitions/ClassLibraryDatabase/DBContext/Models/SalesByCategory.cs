using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Text.RegularExpressions;

namespace WEBtransitions.ClassLibraryDatabase.DBContext;

/// <summary>
/// SELECT Categories.CategoryID, Categories.CategoryName, Products.ProductName, Sum([Order Details Extended].ExtendedPrice) AS ProductSales
/// FROM Categories
///    JOIN Products ON Categories.CategoryID = Products.CategoryID
///    JOIN [Order Details Extended] ON Products.ProductID = [Order Details Extended].ProductID
///    JOIN Orders ON Orders.OrderID = [Order Details Extended].OrderID
/// WHERE Orders.OrderDate BETWEEN DATETIME('2016-01-01') And DATETIME('2016-12-31')
/// GROUP BY Categories.CategoryID, Categories.CategoryName, Products.ProductName;
/// </summary>
public partial class SalesByCategory
{
    public int CategoryId { get; set; }

    public required string CategoryName { get; set; }

    public string? ProductName { get; set; }

    public decimal? ProductSales { get; set; }

    internal static void Configure(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SalesByCategory>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Sales by Category");

            entity.Property(e => e.CategoryId).HasColumnName("CategoryID").HasColumnType("INTEGER");
            entity.Property(e => e.CategoryName).IsRequired().HasColumnType("TEXT").HasMaxLength(15);
            entity.Property(e => e.ProductName).HasColumnType("TEXT").HasMaxLength(40);
            entity.Property(e => e.ProductSales).HasColumnType("REAL");
        });
    }
}
