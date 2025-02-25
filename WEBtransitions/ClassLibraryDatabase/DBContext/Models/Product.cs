using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace WEBtransitions.ClassLibraryDatabase.DBContext;

public partial class Product
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

    public virtual Category? Category { get; set; }
    public virtual Supplier? Supplier { get; set; }
    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();


    internal static void Configure(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>(entity =>
        {
            entity
                .ToTable("Products")
                .HasKey(e => e.ProductId).HasName("PK_Products");
            entity.HasIndex(e => e.CategoryId).IsUnique(false).HasDatabaseName("CategoriesProducts");
            entity.HasIndex(e => e.ProductName).IsUnique(false).HasDatabaseName("IX_ProductName");
            entity.HasIndex(e => e.SupplierId).IsUnique(false).HasDatabaseName("SuppliersProducts");

            entity.Property(e => e.ProductId).HasColumnName("ProductID").HasColumnType("INTEGER").ValueGeneratedOnAdd();
            entity.Property(e => e.ProductName).HasColumnType("TEXT").HasMaxLength(40);
            entity.Property(e => e.SupplierId).HasColumnName("SupplierID");
            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.QuantityPerUnit).HasColumnType("TEXT").HasMaxLength(20);
            entity.Property(e => e.UnitPrice).HasDefaultValue(0.0).HasColumnType("REAL");
            entity.Property(e => e.UnitsInStock).HasColumnType("INTEGER");
            entity.Property(e => e.UnitsOnOrder).HasColumnType("INTEGER");
            entity.Property(e => e.ReorderLevel).HasColumnType("INTEGER");
            entity.Property(e => e.Discontinued).HasDefaultValue("0").HasColumnType("TEXT").HasMaxLength(1);

            entity.HasOne(d => d.Category).WithMany(p => p.Products).HasForeignKey(d => d.CategoryId);
            entity.HasOne(d => d.Supplier).WithMany(p => p.Products).HasForeignKey(d => d.SupplierId);
        });
    }
}
