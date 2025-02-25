using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace WEBtransitions.ClassLibraryDatabase.DBContext;

public partial class Territory
{
    public required string TerritoryId { get; set; }

    public required string TerritoryDescription { get; set; }

    public int RegionId { get; set; }

    public virtual Region Region { get; set; } = null!;

    /// <summary>
    /// M-M relation to Employee object
    /// </summary>
    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();

    internal static void Configure(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Territory>(entity =>
        {
            entity
                .ToTable("Territories")
                .HasKey(e => e.TerritoryId).HasName("PK_Territories");

            entity.Property(e => e.TerritoryId).HasColumnName("TerritoryID").HasColumnType("TEXT").HasMaxLength(20);
            entity.Property(e => e.TerritoryDescription).HasColumnType("TEXT").HasMaxLength(50);
            entity.Property(e => e.RegionId).HasColumnName("RegionID").HasColumnType("INTEGER");

            entity.HasOne(d => d.Region).WithMany(p => p.Territories)
                .HasForeignKey(d => d.RegionId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });
    }
}
