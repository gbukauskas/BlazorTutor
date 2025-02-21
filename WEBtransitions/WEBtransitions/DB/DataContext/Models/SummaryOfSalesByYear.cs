using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace WEBtransitions.DB.DataContext.Models;

/// <summary>
/// SELECT Orders.ShippedDate, Orders.OrderID, [Order Subtotals].Subtotal
/// FROM Orders
///     INNER JOIN[Order Subtotals] ON Orders.OrderID = [Order Subtotals].OrderID
/// WHERE Orders.ShippedDate IS NOT NULL
/// </summary>
public partial class SummaryOfSalesByYear
{
    public DateTime? ShippedDate { get; set; }

    public int OrderId { get; set; }

    public decimal? Subtotal { get; set; }

    internal static void Configure(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SummaryOfSalesByYear>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Summary of Sales by Year");

            entity.Property(e => e.ShippedDate).HasColumnType("DATETIME");
            entity.Property(e => e.OrderId).HasColumnName("OrderID");
            entity.Property(e => e.Subtotal).HasColumnType("REAL");
        });
    }
}
