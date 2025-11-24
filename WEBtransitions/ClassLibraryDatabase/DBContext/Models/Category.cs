using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using WEBtransitions.ClassLibraryDatabase.CustomFilter;

namespace WEBtransitions.ClassLibraryDatabase.DBContext;

public partial class Category
{
    [AllowFiltering]
    public int? CategoryId { get; set; }

    [NotMapped]
    public string ItemKey
    {

        get
        {
            return CategoryId.HasValue ? $"{CategoryId.Value:D6}" : String.Empty;
        }
    }

    [AllowFiltering]
    public string? CategoryName { get; set; } = "";

    [AllowFiltering]
    public string? Description { get; set; }

    public byte[]? Picture { get; set; }

    [NotMapped]
    public string PictureUrl
    {
        get
        {
            if (Picture != null)
            {
                var b64String = Convert.ToBase64String(this.Picture);
                return $"data:image/jpg;base64,{b64String}";
            }
            else
            {
                return "/Images/anonymous.png";
            }
        }
    }
    [NotMapped]
    public IBrowserFile? PictureFile { get; set; }

    public byte IsDeleted { get; set; }

    public int Version { get; set; }
    public bool IgnoreConcurency { get; set; } = false;
    public bool RememberRegion { get; set; } = false;

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
    static internal void Configure(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(entity =>
        {
            entity
                .ToTable("Categories")
                .HasKey(e => e.CategoryId).HasName("PK_Categories");
            entity.HasIndex(e => e.CategoryName).IsUnique(false).HasDatabaseName("IX_CategoryName");

            entity.Ignore(t => t.IgnoreConcurency);
            entity.Ignore(t => t.RememberRegion);

            entity.Property(e => e.CategoryId).HasColumnName("CategoryID").ValueGeneratedOnAdd();
            entity.Property(e => e.CategoryName).IsRequired().HasColumnType("TEXT").HasMaxLength(15);
            entity.Property(e => e.Description).HasColumnType("TEXT").HasColumnType("TEXT");
            entity.Property(e => e.Picture).HasColumnType("BLOB");

            entity.Property(e => e.IsDeleted).HasColumnType("INTEGER").HasDefaultValue(0);
            entity.Property(e => e.Version).HasColumnType("INTEGER").HasDefaultValue(0).IsRowVersion();
        });
    }
}
