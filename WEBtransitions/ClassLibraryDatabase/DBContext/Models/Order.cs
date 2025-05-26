using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace WEBtransitions.ClassLibraryDatabase.DBContext;

public partial class Order
{
    public int OrderId { get; set; }

    public string? CustomerId { get; set; }

    public int? EmployeeId { get; set; }

    public DateTime? OrderDate { get; set; }

    public DateTime? RequiredDate { get; set; }

    public DateTime? ShippedDate { get; set; }

    public int? ShipVia { get; set; }

    public int? Freight { get; set; }

    public string? ShipName { get; set; }

    public string? ShipAddress { get; set; }

    public string? ShipCity { get; set; }

    public string? ShipRegion { get; set; }

    public string? ShipPostalCode { get; set; }

    public string? ShipCountry { get; set; }

    public byte IsDeleted { get; set; }

    public int Version { get; set; }

    public virtual Customer? Customer { get; set; }
    public virtual Employee? Employee { get; set; }
    public virtual Shipper? ShipViaNavigation { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();


    internal static void Configure(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Order>(entity =>
        {
            entity
                .ToTable("Orders")
                .HasKey(e => e.OrderId).HasName("PK_Orders");

            entity.HasIndex(e => e.CustomerId).IsUnique(false).HasDatabaseName("CustomersOrders");
            entity.HasIndex(e => e.EmployeeId).IsUnique(false).HasDatabaseName("EmployeesOrders");
            entity.HasIndex(e => e.OrderDate).IsUnique(false).HasDatabaseName("OrderDate");
            entity.HasIndex(e => e.ShippedDate).IsUnique(false).HasDatabaseName("ShippedDate");
            entity.HasIndex(e => e.ShipVia).IsUnique(false).HasDatabaseName("ShippersOrders");
            entity.HasIndex(e => e.ShipPostalCode).IsUnique(false).HasDatabaseName("ShipPostalCode");

            entity.Property(e => e.OrderId).HasColumnName("OrderID").HasColumnType("INTEGER");
            entity.Property(e => e.CustomerId).HasColumnName("CustomerID").HasColumnType("TEXT").HasMaxLength(5);
            entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID").HasColumnType("INTEGER");
            entity.Property(e => e.OrderDate).HasColumnType("DATETIME");
            entity.Property(e => e.RequiredDate).HasColumnType("DATETIME");
            entity.Property(e => e.ShippedDate).HasColumnType("DATETIME");
            entity.Property(e => e.ShipVia).HasColumnType("INTEGER");
            entity.Property(e => e.Freight).HasDefaultValue(0).HasColumnType("NUMERIC");
            entity.Property(e => e.ShipName).HasColumnType("TEXT").HasMaxLength(40);
            entity.Property(e => e.ShipAddress).HasColumnType("TEXT").HasMaxLength(60);
            entity.Property(e => e.ShipCity).HasColumnType("TEXT").HasMaxLength(15);
            entity.Property(e => e.ShipRegion).HasColumnType("TEXT").HasMaxLength(15);
            entity.Property(e => e.ShipPostalCode).HasColumnType("TEXT").HasMaxLength(10);
            entity.Property(e => e.ShipCountry).HasColumnType("TEXT").HasMaxLength(15);

            entity.Property(e => e.IsDeleted).HasColumnType("INTEGER").HasDefaultValue(0);
            entity.Property(e => e.Version).HasColumnType("INTEGER").HasDefaultValue(0).IsRowVersion();

            entity.HasOne(d => d.Customer).WithMany(p => p.Orders).HasForeignKey(d => d.CustomerId).HasConstraintName("FK_Orders_Customers");
            entity.HasOne(d => d.Employee).WithMany(p => p.Orders).HasForeignKey(d => d.EmployeeId).HasConstraintName("FK_Orders_Employees");
            entity.HasOne(d => d.ShipViaNavigation).WithMany(p => p.Orders).HasForeignKey(d => d.ShipVia).HasConstraintName("FK_Orders_Shippers");
        });
    }
}
