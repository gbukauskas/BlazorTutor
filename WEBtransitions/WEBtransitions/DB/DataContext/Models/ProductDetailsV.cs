using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace WEBtransitions.DB.DataContext.Models;

public partial class ProductDetailsV
{
    public int ProductId { get; set; }
    public required string ProductName { get; set; }
    public int? SupplierId { get; set; }
    public int? CategoryId { get; set; }
    public string? QuantityPerUnit { get; set; }
    public double? UnitPrice { get; set; }
    public short? UnitsInStock { get; set; }
    public short? UnitsOnOrder { get; set; }
    public short? ReorderLevel { get; set; }
    public required string Discontinued { get; set; }
    public required string CategoryName { get; set; }
    public string? CategoryDescription { get; set; }
    public required string SupplierName { get; set; }
    public string? SupplierRegion { get; set; }

    internal static void Configure(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ProductDetailsV>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("ProductDetails_V");

            entity.Property(e => e.ProductId).HasColumnName("ProductID").HasColumnType("INTEGER");
            entity.Property(e => e.ProductName).HasColumnType("TEXT").HasMaxLength(40);
            entity.Property(e => e.SupplierId).HasColumnName("SupplierID").HasColumnType("INTEGER");
            entity.Property(e => e.CategoryId).HasColumnName("CategoryID").HasColumnType("INTEGER");
            entity.Property(e => e.QuantityPerUnit).HasColumnType("TEXT").HasMaxLength(20);
            entity.Property(e => e.UnitPrice).HasColumnType("NUMERIC");
            entity.Property(e => e.UnitsInStock).HasColumnType("NUMERIC");
            entity.Property(e => e.UnitsOnOrder).HasColumnType("NUMERIC");
            entity.Property(e => e.ReorderLevel).HasColumnType("NUMERIC");
            entity.Property(e => e.Discontinued).HasColumnType("TEXT").HasMaxLength(1);
            entity.Property(e => e.CategoryName).HasColumnType("TEXT").HasMaxLength(15);
            entity.Property(e => e.CategoryDescription).HasColumnType("TEXT");
            entity.Property(e => e.SupplierName).HasColumnType("TEXT").HasMaxLength(40);
            entity.Property(e => e.SupplierRegion).HasColumnType("TEXT").HasMaxLength(15);
        });
    }
}
