using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace WEBtransitions.ClassLibraryDatabase.DBContext;

public partial class CurrentProductList
{
    public int ProductId { get; set; }

    public required string ProductName { get; set; }

    static internal void Configure(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CurrentProductList>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Current Product List");

            entity.Property(e => e.ProductId)
                .HasColumnName("ProductID")
                .IsRequired();
            entity.Property(e => e.ProductName)
                .IsRequired()
                .HasMaxLength(40);
        });
    }

}
