using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace WEBtransitions.ClassLibraryDatabase.DBContext;

public partial class OrderDetailsExtended
{
    public int OrderId { get; set; }

    public int ProductId { get; set; }

    public required string ProductName { get; set; }

    public double UnitPrice { get; set; }

    public short Quantity { get; set; }

    public double? Discount { get; set; }

    public double? ExtendedPrice { get; set; }

    internal static void Configure(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<OrderDetailsExtended>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Order Details Extended");

            entity.Property(e => e.OrderId).HasColumnName("OrderID").HasColumnType("INTEGER");
            entity.Property(e => e.ProductId).HasColumnName("ProductID").HasColumnType("INTEGER");
            entity.Property(e => e.ProductName).HasColumnType("TEXT");
            entity.Property(e => e.UnitPrice).HasColumnType("NUMERIC");
            entity.Property(e => e.Quantity).HasColumnType("INTEGER");
            entity.Property(e => e.Discount).HasColumnType("REAL");
            entity.Property(e => e.Discount).HasColumnType("NUMERIC");
        });
    }
}
