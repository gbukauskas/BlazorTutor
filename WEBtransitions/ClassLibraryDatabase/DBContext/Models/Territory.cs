using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WEBtransitions.ClassLibraryDatabase.DBContext;

public partial class Territory
{
#pragma warning disable CS8618
    public string TerritoryId { get; set; }

    public required string TerritoryDescription { get; set; }

    public int RegionId { get; set; }

    /// <summary>
    /// [dbo].[Region].[RegionDescription]
    /// </summary>
    public string? RegionDescription { get; set; }

    public byte IsDeleted { get; set; }
    public bool IgnoreConcurency { get; set; } = false;
    public int Version { get; set; }

    public virtual Region Region { get; set; } = null!;
#pragma warning restore CS8618
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

            entity.Ignore(t => t.IgnoreConcurency);


            entity.Property(e => e.TerritoryId).HasColumnName("TerritoryID").HasColumnType("TEXT").HasMaxLength(20);
            entity.Property(e => e.TerritoryDescription).HasColumnType("TEXT").HasMaxLength(50);
            entity.Property(e => e.RegionId).HasColumnName("RegionID").HasColumnType("INTEGER");

            entity.Property(e => e.IsDeleted).HasColumnType("INTEGER").HasDefaultValue(0);
            entity.Property(e => e.Version).HasColumnType("INTEGER").HasDefaultValue(0).IsRowVersion();

            entity.HasOne(d => d.Region).WithMany(p => p.Territories)
                .HasForeignKey(d => d.RegionId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });
    }
}
