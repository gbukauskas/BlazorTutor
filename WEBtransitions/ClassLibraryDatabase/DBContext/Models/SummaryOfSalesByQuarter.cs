using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace WEBtransitions.ClassLibraryDatabase.DBContext;

/// <summary>
/// SELECT Orders.ShippedDate, Orders.OrderID, [Order Subtotals].Subtotal
/// FROM Orders
///     INNER JOIN[Order Subtotals] ON Orders.OrderID = [Order Subtotals].OrderID
/// WHERE Orders.ShippedDate IS NOT NULL
/// </summary>
public partial class SummaryOfSalesByQuarter
{
    public DateTime? ShippedDate { get; set; }

    public int OrderId { get; set; }

    public decimal? Subtotal { get; set; }

    internal static void Configure(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SummaryOfSalesByQuarter>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Summary of Sales by Quarter");

            entity.Property(e => e.ShippedDate).HasColumnType("DATETIME");
            entity.Property(e => e.OrderId).HasColumnName("OrderID").HasColumnType("INTEGER");
            entity.Property(e => e.Subtotal).HasColumnType("REAL");
        });
    }
}
