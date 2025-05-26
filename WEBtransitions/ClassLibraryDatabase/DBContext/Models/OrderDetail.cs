using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace WEBtransitions.ClassLibraryDatabase.DBContext;

public partial class OrderDetail
{
    public int OrderId { get; set; }

    public int ProductId { get; set; }

    public double UnitPrice { get; set; }

    public short Quantity { get; set; }

    public double Discount { get; set; }

    public byte IsDeleted { get; set; }

    public int Version { get; set; }

    public virtual Order Order { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;

    internal static void Configure(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.HasKey(e => new { e.OrderId, e.ProductId }).HasName("PK_Order_Details");
            entity.ToTable("Order Details");

            entity.Property(e => e.OrderId).HasColumnName("OrderID").HasColumnType("INTEGER");
            entity.Property(e => e.ProductId).HasColumnName("ProductID").HasColumnType("INTEGER");
            entity.Property(e => e.UnitPrice).HasColumnType("NUMERIC");
            entity.Property(e => e.Quantity).HasDefaultValue(1).HasColumnType("INTEGER");
            entity.Property(e => e.Discount).HasColumnType("REAL");

            entity.Property(e => e.IsDeleted).HasColumnType("INTEGER").HasDefaultValue(0);
            entity.Property(e => e.Version).HasColumnType("INTEGER").HasDefaultValue(0).IsRowVersion();

            entity.HasOne(d => d.Order).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.OrderId).HasConstraintName("FK_Order_Details_Orders")
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Product).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.ProductId).HasConstraintName("FK_Order_Details_Products")
                .OnDelete(DeleteBehavior.ClientSetNull);
        });
    }
}
