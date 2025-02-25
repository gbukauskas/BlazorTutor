using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace WEBtransitions.ClassLibraryDatabase.DBContext;

public partial class Employee
{
    public int EmployeeId { get; set; }

    public required string LastName { get; set; }

    public required string FirstName { get; set; }

    public string? Title { get; set; }

    public string? TitleOfCourtesy { get; set; }

    public DateOnly? BirthDate { get; set; }

    public DateOnly? HireDate { get; set; }

    public string? Address { get; set; }

    public string? City { get; set; }

    public string? Region { get; set; }

    public string? PostalCode { get; set; }

    public string? Country { get; set; }

    public string? HomePhone { get; set; }

    public string? Extension { get; set; }

    public byte[]? Photo { get; set; }

    public string? Notes { get; set; }

    public int? ReportsTo { get; set; }

    public string? PhotoPath { get; set; }

    public virtual ICollection<Employee> InverseReportsToNavigation { get; set; } = new List<Employee>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual Employee? ReportsToNavigation { get; set; }

    /// <summary>
    /// M-M relation to Territory object
    /// </summary>
    public virtual ICollection<Territory> Territories { get; set; } = new List<Territory>();

    internal static void Configure(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Employee>(entity =>
        {
            entity
                .ToTable("Employees")
                .HasKey(e => e.EmployeeId).HasName("PK_Employees");

            entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID").HasColumnType("INTEGER").ValueGeneratedOnAdd();
            entity.Property(e => e.LastName).IsRequired().HasMaxLength(20).HasColumnType("TEXT");
            entity.Property(e => e.FirstName).IsRequired().HasMaxLength(10).HasColumnType("TEXT");
            entity.Property(e => e.Title).HasMaxLength(30).HasColumnType("TEXT");
            entity.Property(e => e.TitleOfCourtesy).HasMaxLength(25).HasColumnType("TEXT");
            entity.Property(e => e.BirthDate).HasColumnType("DATE");
            entity.Property(e => e.HireDate).HasColumnType("DATE");
            entity.Property(e => e.Address).HasMaxLength(60).HasColumnType("TEXT");
            entity.Property(e => e.City).HasMaxLength(15).HasColumnType("TEXT");
            entity.Property(e => e.Region).HasMaxLength(15).HasColumnType("TEXT");
            entity.Property(e => e.PostalCode).HasMaxLength(10).HasColumnType("TEXT");
            entity.Property(e => e.Country).HasMaxLength(15).HasColumnType("TEXT");
            entity.Property(e => e.HomePhone).HasMaxLength(24).HasColumnType("TEXT");
            entity.Property(e => e.Extension).HasMaxLength(4).HasColumnType("TEXT");
            entity.Property(e => e.Photo).HasColumnType("BLOB");
            entity.Property(e => e.Notes).HasColumnType("TEXT");
            entity.Property(e => e.ReportsTo).HasColumnType("INTEGER");
            entity.Property(e => e.PhotoPath).HasMaxLength(255).HasColumnType("TEXT");

            entity.HasOne(d => d.ReportsToNavigation).WithMany(p => p.InverseReportsToNavigation).HasForeignKey(d => d.ReportsTo);

            entity.HasMany(d => d.Territories).WithMany(p => p.Employees)
                .UsingEntity<Dictionary<string, object>>(
                    "EmployeeTerritory",
                    r => r.HasOne<Territory>().WithMany()
                        .HasForeignKey("TerritoryId")
                        .OnDelete(DeleteBehavior.ClientSetNull),
                    l => l.HasOne<Employee>().WithMany()
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.ClientSetNull),
                    j =>
                    {
                        j.HasKey("EmployeeId", "TerritoryId");
                        j.ToTable("EmployeeTerritories");
                        j.IndexerProperty<int>("EmployeeId").HasColumnName("EmployeeID");
                        j.IndexerProperty<string>("TerritoryId").HasColumnName("TerritoryID");
                    });
        });
    }
}
