using Microsoft.EntityFrameworkCore;

namespace WEBtransitions.ClassLibraryDatabase.DBContext;

public partial class EmployeeTerritory
{
    public int EmployeeId { get; set; }
    public int TerritoryId { get; set; }

    internal static void Configure(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<EmployeeTerritory>(entity =>
        {
            entity
                .ToTable("EmployeeTerritories")
                .HasKey(e => new { e.EmployeeId, e.TerritoryId }).HasName("PK_EmployeeTerritories");

            entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID").HasColumnType("INTEGER");
            entity.Property(e => e.TerritoryId).HasColumnName("EmployeeID").HasColumnType("INTEGER");

        });
    }
}
