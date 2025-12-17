using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace WEBtransitions.ClassLibraryDatabase.DBContext;

public partial class Region
{
    public int? RegionId { get; set; }

    [NotMapped]
    public string ItemKey
    {

        get
        {
            return RegionId.HasValue ? $"{RegionId.Value:D6}" : String.Empty;
        }
    }

    public required string RegionDescription { get; set; }

    public byte IsDeleted { get; set; }

    public int Version { get; set; }
    public bool IgnoreConcurency { get; set; } = false;

    public virtual ICollection<Territory> Territories { get; set; } = [];

    internal static void Configure(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Region>(entity =>
        {
            entity
                .ToTable("Regions")
                .HasKey(e => e.RegionId).HasName("PK_Region");
            
            entity.Ignore(t => t.IgnoreConcurency);

            entity.Property(e => e.RegionId).IsRequired().HasColumnType("INTEGER").ValueGeneratedOnAdd().HasColumnName("RegionID");
            entity.Property(e => e.RegionDescription).HasColumnType("TEXT").HasMaxLength(50);

            entity.Property(e => e.IsDeleted).HasColumnType("INTEGER").HasDefaultValue(0);
            entity.Property(e => e.Version).HasColumnType("INTEGER").HasDefaultValue(0).IsRowVersion();
        });
    }
    public void AssignProperties(Region src)
    {
        this.RegionId = src.RegionId;
        this.RegionDescription = src.RegionDescription;
        this.IsDeleted = src.IsDeleted;

        this.Version = src.IgnoreConcurency
            ? -1    // Ignore concurrency error
            : src.Version;
    }
}
