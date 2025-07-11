﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace WEBtransitions.ClassLibraryDatabase.DBContext;

public partial class Region
{
    public int RegionId { get; set; }

    public required string RegionDescription { get; set; }

    public byte IsDeleted { get; set; }

    public int Version { get; set; }

    public virtual ICollection<Territory> Territories { get; set; } = new List<Territory>();

    internal static void Configure(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Region>(entity =>
        {
            entity
                .ToTable("Regions")
                .HasKey(e => e.RegionId).HasName("PK_Region");

            entity.Property(e => e.RegionId)
                .ValueGeneratedNever()
                .HasColumnName("RegionID");
            entity.Property(e => e.RegionDescription).HasColumnType("TEXT").HasMaxLength(50);

            entity.Property(e => e.IsDeleted).HasColumnType("INTEGER").HasDefaultValue(0);
            entity.Property(e => e.Version).HasColumnType("INTEGER").HasDefaultValue(0).IsRowVersion();
        });
    }
}
