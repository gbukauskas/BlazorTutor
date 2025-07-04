﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace WEBtransitions.ClassLibraryDatabase.DBContext;

public partial class Category
{
    public int CategoryId { get; set; }

    public required string CategoryName { get; set; }

    public string? Description { get; set; }

    public byte[]? Picture { get; set; }

    public byte IsDeleted { get; set; }

    public int Version { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
    static internal void Configure(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(entity =>
        {
            entity
                .ToTable("Categories")
                .HasKey(e => e.CategoryId).HasName("PK_Categories");
            entity.HasIndex(e => e.CategoryName).IsUnique(false).HasDatabaseName("IX_CategoryName");

            entity.Property(e => e.CategoryId).HasColumnName("CategoryID").ValueGeneratedOnAdd();
            entity.Property(e => e.CategoryName).IsRequired().HasColumnType("TEXT").HasMaxLength(15);
            entity.Property(e => e.Description).HasColumnType("TEXT").HasColumnType("TEXT");
            entity.Property(e => e.Picture).HasColumnType("BLOB");

            entity.Property(e => e.IsDeleted).HasColumnType("INTEGER").HasDefaultValue(0);
            entity.Property(e => e.Version).HasColumnType("INTEGER").HasDefaultValue(0).IsRowVersion();
        });
    }
}
