using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace WEBtransitions.DB.DataContext.Models;

public partial class NorthwindContext : DbContext
{
    public NorthwindContext()
    {
    }

    public NorthwindContext(DbContextOptions<NorthwindContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AlphabeticalListOfProduct> AlphabeticalListOfProducts { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<CategorySalesFor1997> CategorySalesFor1997s { get; set; }

    public virtual DbSet<CurrentProductList> CurrentProductLists { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<CustomerAndSuppliersByCity> CustomerAndSuppliersByCities { get; set; }

    public virtual DbSet<CustomerDemographic> CustomerDemographics { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<Invoice> Invoices { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderDetail> OrderDetails { get; set; }

    public virtual DbSet<OrderDetailsExtended> OrderDetailsExtendeds { get; set; }

    public virtual DbSet<OrderSubtotal> OrderSubtotals { get; set; }

    public virtual DbSet<OrdersQry> OrdersQries { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductDetailsV> ProductDetailsVs { get; set; }

    public virtual DbSet<ProductSalesFor1997> ProductSalesFor1997s { get; set; }

    public virtual DbSet<ProductsAboveAveragePrice> ProductsAboveAveragePrices { get; set; }

    public virtual DbSet<ProductsByCategory> ProductsByCategories { get; set; }

    public virtual DbSet<QuarterlyOrder> QuarterlyOrders { get; set; }

    public virtual DbSet<Region> Regions { get; set; }

    public virtual DbSet<SalesByCategory> SalesByCategories { get; set; }

    public virtual DbSet<SalesTotalsByAmount> SalesTotalsByAmounts { get; set; }

    public virtual DbSet<Shipper> Shippers { get; set; }

    public virtual DbSet<SummaryOfSalesByQuarter> SummaryOfSalesByQuarters { get; set; }

    public virtual DbSet<SummaryOfSalesByYear> SummaryOfSalesByYears { get; set; }

    public virtual DbSet<Supplier> Suppliers { get; set; }

    public virtual DbSet<Territory> Territories { get; set; }
/*
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlite("Data Source=DB\\northwind.db");
*/
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        AlphabeticalListOfProduct.Configure(modelBuilder);
        Category.Configure(modelBuilder);
        CategorySalesFor1997.Configure(modelBuilder);
        CurrentProductList.Configure(modelBuilder);
        Customer.Configure(modelBuilder);
        CustomerAndSuppliersByCity.Configure(modelBuilder);
        CustomerDemographic.Configure(modelBuilder);
        Employee.Configure(modelBuilder);
        Invoice.Configure(modelBuilder);
        Order.Configure(modelBuilder);
        OrderDetail.Configure(modelBuilder);
        OrderDetailsExtended.Configure(modelBuilder);
        OrderSubtotal.Configure(modelBuilder);
        OrdersQry.Configure(modelBuilder);
        Product.Configure(modelBuilder);
        ProductDetailsV.Configure(modelBuilder);
        ProductSalesFor1997.Configure(modelBuilder);
        ProductsAboveAveragePrice.Configure(modelBuilder);
        ProductsByCategory.Configure(modelBuilder);
        QuarterlyOrder.Configure(modelBuilder);
        Region.Configure(modelBuilder);
        SalesByCategory.Configure(modelBuilder);
        SalesTotalsByAmount.Configure(modelBuilder);
        Shipper.Configure(modelBuilder);
        SummaryOfSalesByQuarter.Configure(modelBuilder);
        SummaryOfSalesByYear.Configure(modelBuilder);
        Supplier.Configure(modelBuilder);
        Territory.Configure(modelBuilder);

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
