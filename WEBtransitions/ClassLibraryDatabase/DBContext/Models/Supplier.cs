using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace WEBtransitions.ClassLibraryDatabase.DBContext;

public partial class Supplier
{
    public int SupplierId { get; set; }

    public required string CompanyName { get; set; }

    public string? ContactName { get; set; }

    public string? ContactTitle { get; set; }

    public string? Address { get; set; }

    public string? City { get; set; }

    public string? Region { get; set; }

    public string? PostalCode { get; set; }

    public string? Country { get; set; }

    public string? Phone { get; set; }

    public string? Fax { get; set; }

    public string? HomePage { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();

    internal static void Configure(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Supplier>(entity =>
        {
            entity
                .ToTable("Suppliers")
                .HasKey(e => e.SupplierId).HasName("PK_Suppliers");
            entity.HasIndex(e => e.CompanyName).IsUnique(false).HasDatabaseName("CompanyName");
            entity.HasIndex(e => e.PostalCode).IsUnique(false).HasDatabaseName("PostalCode");

            entity.Property(e => e.SupplierId).HasColumnName("SupplierID").HasColumnType("INTEGER").ValueGeneratedOnAdd();
            entity.Property(e => e.CompanyName).IsRequired().HasColumnType("TEXT").HasMaxLength(40);
            entity.Property(e => e.ContactName).HasColumnType("TEXT").HasMaxLength(30);
            entity.Property(e => e.ContactTitle).HasColumnType("TEXT").HasMaxLength(30);
            entity.Property(e => e.Address).HasColumnType("TEXT").HasMaxLength(60);
            entity.Property(e => e.City).HasColumnType("TEXT").HasMaxLength(15);
            entity.Property(e => e.Region).HasColumnType("TEXT").HasMaxLength(15);
            entity.Property(e => e.PostalCode).HasColumnType("TEXT").HasMaxLength(10);
            entity.Property(e => e.Country).HasColumnType("TEXT").HasMaxLength(15);
            entity.Property(e => e.Phone).HasColumnType("TEXT").HasMaxLength(24);
            entity.Property(e => e.Fax).HasColumnType("TEXT").HasMaxLength(24);
            entity.Property(e => e.HomePage).HasColumnType("TEXT");
        });
    }
}
