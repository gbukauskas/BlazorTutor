using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace WEBtransitions.ClassLibraryDatabase.DBContext;

/// <summary>
/// SELECT [Order Subtotals].Subtotal AS SaleAmount, Orders.OrderID, Customers.CompanyName, Orders.ShippedDate
/// FROM Customers
///     JOIN Orders ON Customers.CustomerID = Orders.CustomerID
///     JOIN[Order Subtotals] ON Orders.OrderID = [Order Subtotals].OrderID
/// WHERE ([Order Subtotals].Subtotal >2500)
///     AND(Orders.ShippedDate BETWEEN DATETIME('2016-01-01') And DATETIME('2016-12-31'))
/// </summary>
public partial class SalesTotalsByAmount
{
    public decimal? SaleAmount { get; set; }

    public int OrderId { get; set; }

    public required string CompanyName { get; set; }

    public DateTime? ShippedDate { get; set; }

    internal static void Configure(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SalesTotalsByAmount>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Sales Totals by Amount");

            entity.Property(e => e.SaleAmount).HasColumnType("REAL");
            entity.Property(e => e.OrderId).HasColumnName("OrderID").HasColumnType("INTEGER");
            entity.Property(e => e.CompanyName).IsRequired().HasColumnType("TEXT").HasMaxLength(40);
            entity.Property(e => e.ShippedDate).HasColumnType("DATETIME");

        });
    }
}
