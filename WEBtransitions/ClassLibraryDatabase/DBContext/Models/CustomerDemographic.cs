using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace WEBtransitions.ClassLibraryDatabase.DBContext;

public partial class CustomerDemographic
{
    public required string CustomerTypeId { get; set; }

    public string? CustomerDesc { get; set; }

    public virtual ICollection<Customer> Customers { get; set; } = new List<Customer>();

    internal static void Configure(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CustomerDemographic>(entity =>
        {
            entity.HasKey(e => e.CustomerTypeId).HasName("PK_CustomerDemographics");

            entity.Property(e => e.CustomerTypeId).HasColumnType("TEXT").HasColumnName("CustomerTypeID").HasMaxLength(10);
            entity.Property(e => e.CustomerDesc).HasColumnType("TEXT");
        });
    }

}
