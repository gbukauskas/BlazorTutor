using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace WEBtransitions.ClassLibraryDatabase.DBContext;

public partial class Shipper
{
    public int ShipperId { get; set; }

    public required string CompanyName { get; set; }

    public string? Phone { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    internal static void Configure(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Shipper>(entity =>
        {
            entity
                .ToTable("Shippers")
                .HasKey(e => e.ShipperId).HasName("PK_Shippers");

            entity.Property(e => e.ShipperId).HasColumnName("ShipperID").HasColumnType("INTEGER").ValueGeneratedOnAdd();
            entity.Property(e => e.CompanyName).IsRequired().HasColumnType("TEXT").HasMaxLength(40);
            entity.Property(e => e.Phone).HasColumnType("TEXT").HasMaxLength(24);
        });
    }
}
