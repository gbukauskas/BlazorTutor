using Microsoft.EntityFrameworkCore;

namespace WEBtransitions.ClassLibraryDatabase.DBContext;

/// <summary>
///     CREATE VIEW [Alphabetical list of products] AS 
///         SELECT Products.*, Categories.CategoryName 
///         FROM Categories 
///         INNER JOIN Products ON Categories.CategoryID = Products.CategoryID 
///         WHERE (((Products.Discontinued)=0))
/// </summary>
public partial class AlphabeticalListOfProduct
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

    public required string CategoryName { get; set; }

    static internal void Configure(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AlphabeticalListOfProduct>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Alphabetical list of products");

            entity.Property(e => e.ProductId).HasColumnName("ProductID").IsRequired();
            entity.Property(e => e.ProductName).IsRequired().HasColumnType("TEXT").HasMaxLength(40);
            entity.Property(e => e.SupplierId).HasColumnName("SupplierID");
            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.QuantityPerUnit).HasColumnType("TEXT").HasMaxLength(20);
            entity.Property(e => e.UnitPrice).HasColumnType("NUMERIC");
            entity.Property(e => e.UnitsInStock).HasColumnType("INTEGER");
            entity.Property(e => e.UnitsOnOrder).HasColumnType("INTEGER");
            entity.Property(e => e.ReorderLevel).HasColumnType("INTEGER");
            entity.Property(e => e.Discontinued).HasColumnType("TEXT").IsRequired().HasDefaultValue("0").HasMaxLength(1);
            entity.Property(e => e.CategoryName).HasColumnType("TEXT").IsRequired().HasMaxLength(15);
        });
    }
}
