﻿using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.Intrinsics.Arm;
using WEBtransitions.ClassLibraryDatabase.CustomFilter;
using WEBtransitions.ClassLibraryDatabase.CustomPager;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace WEBtransitions.ClassLibraryDatabase.DBContext;

public partial class Customer
{
    public required string CustomerId { get; set; }

    [AllowFiltering]
    public required string CompanyName { get; set; }

    [AllowFiltering]
    public string? ContactName { get; set; }

    [AllowFiltering]
    public string? ContactTitle { get; set; }

    [AllowFiltering]
    public string? Address { get; set; }

    [AllowFiltering]
    public string? City { get; set; }

    [AllowFiltering]
    public string? Region { get; set; }

    [AllowFiltering]
    public string? PostalCode { get; set; }

    [AllowFiltering]
    public string? Country { get; set; }

    public string? Phone { get; set; }

    public string? Fax { get; set; }

    public virtual ICollection<CustomerDemographic> CustomerTypes { get; set; } = new List<CustomerDemographic>();


    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();


    internal static void Configure(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Customer>(entity =>
        {
            entity
                .ToTable("Customers")
                .HasKey(e => e.CustomerId).HasName("PK_Customers");
            entity.HasIndex(e => e.City).IsUnique(false).HasDatabaseName("City");
            entity.HasIndex(e => e.CompanyName).IsUnique(false).HasDatabaseName("CompanyName");
            entity.HasIndex(e => e.PostalCode).IsUnique(false).HasDatabaseName("PostalCode");
            entity.HasIndex(e => e.Region).IsUnique(false).HasDatabaseName("Region");

            entity.Property(e => e.CustomerId).HasColumnName("CustomerID").IsRequired().HasColumnType("TEXT").HasMaxLength(5);
            entity.Property(e => e.CompanyName).IsRequired().HasColumnType("TEXT").HasMaxLength(40);
            entity.Property(e => e.ContactName).HasColumnType("TEXT").HasMaxLength(30);
            entity.Property(e => e.ContactTitle).HasColumnType("TEXT").HasMaxLength(30);
            entity.Property(e => e.Address).HasColumnType("TEXT").HasMaxLength(60);
            entity.Property(e => e.City).HasColumnType("TEXT").HasMaxLength(15);
            entity.Property(e => e.Region).HasColumnType("TEXT").HasMaxLength(15);
            entity.Property(e => e.PostalCode).HasColumnType("TEXT").HasMaxLength(10);
            entity.Property(e => e.Country).HasColumnType("TEXT").HasMaxLength(15);
            entity.Property(e => e.Phone).HasColumnType("TEXT").HasMaxLength(24);
            entity.Property(e => e.Fax).HasColumnType("TEXT").HasMaxLength(24);



            entity.HasMany(d => d.CustomerTypes).WithMany(p => p.Customers)
                .UsingEntity<Dictionary<string, object>>(
                    "CustomerCustomerDemo",
                    r => r.HasOne<CustomerDemographic>().WithMany()
                        .HasForeignKey("CustomerTypeId")
                        .OnDelete(DeleteBehavior.ClientSetNull),
                    l => l.HasOne<Customer>().WithMany()
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.ClientSetNull),
                    j =>
                    {
                        j.HasKey("CustomerId", "CustomerTypeId");
                        j.ToTable("CustomerCustomerDemo");
                        j.IndexerProperty<string>("CustomerId").HasColumnName("CustomerID");
                        j.IndexerProperty<string>("CustomerTypeId").HasColumnName("CustomerTypeID");
                    });
        });
    }

    public static IEnumerable<Customer> LoadFromDB(SqliteDataReader rdr/*, PgPostData pagerData*/)
    {
        // Returns string? object
        Func<SqliteDataReader, string, string?> safeConvertToString = (aRdr, x) =>
        {
            var value = aRdr[x];
            return value == DBNull.Value ? null : value.ToString();
        };
        // returns total count of pages
        //Func<SqliteConnection, SqliteDataReader, string, int> CountPages = (conn, aRdr, sql) =>
        //{
        //    using (SqliteCommand command = new SqliteCommand(sql.Replace("*", "COUNT(1)"), conn))
        //    {
        //        var rowCountObject = command.ExecuteScalar();
        //        long rowCont = (long)(rowCountObject == null ? 0L : rowCountObject);
        //        pagerData.RowCount = (int)rowCont;

        //        int pgCount = pagerData.RowCount / pagerData.PageSize;
        //        if (pagerData.RowCount % pagerData.PageSize > 0)
        //        {
        //            pgCount += 1;
        //        }

        //        if (pagerData.PageNumber * pagerData.PageSize >= rowCont)
        //        {
        //            pagerData.PageNumber = 1;
        //        }
        //        return pgCount;
        //    }
        //};

        //using (SqliteConnection conn = new SqliteConnection(connStr))
        //{
        //    conn.Open();
        //    using (SqliteCommand readCommand = new SqliteCommand(sqlStr, conn))
        //    using (SqliteDataReader rdr = readCommand.ExecuteReader())
        //    {
        //        pagerData.PageCount = CountPages(conn, rdr, sqlStr);

                while (rdr.Read())
                {
#pragma warning disable CS8601
                    yield return new Customer()
                    {
                        CustomerId = rdr["CustomerID"].ToString(),
                        CompanyName = rdr["CompanyName"].ToString(),
                        ContactName = safeConvertToString(rdr, "ContactName"),
                        ContactTitle = safeConvertToString(rdr, "ContactTitle"),
                        Address = safeConvertToString(rdr, "Address"),
                        City = safeConvertToString(rdr, "City"),
                        Region = safeConvertToString(rdr, "Region"),
                        PostalCode = safeConvertToString(rdr, "PostalCode"),
                        Country = safeConvertToString(rdr, "Country"),
                        Phone = safeConvertToString(rdr, "Phone"),
                        Fax = safeConvertToString(rdr, "Fax")
                    };
#pragma warning restore CS8601
                }
        //    }
        //    conn.Close();
        //}
    }


}

// https://learn.microsoft.com/en-us/ef/core/modeling/relationships/many-to-many
// https://medium.com/@jasminewith/entity-framework-core-many-to-many-relationship-258df60bba74#id_token=eyJhbGciOiJSUzI1NiIsImtpZCI6IjVkMTJhYjc4MmNiNjA5NjI4NWY2OWU0OGFlYTk5MDc5YmI1OWNiODYiLCJ0eXAiOiJKV1QifQ.eyJpc3MiOiJodHRwczovL2FjY291bnRzLmdvb2dsZS5jb20iLCJhenAiOiIyMTYyOTYwMzU4MzQtazFrNnFlMDYwczJ0cDJhMmphbTRsamRjbXMwMHN0dGcuYXBwcy5nb29nbGV1c2VyY29udGVudC5jb20iLCJhdWQiOiIyMTYyOTYwMzU4MzQtazFrNnFlMDYwczJ0cDJhMmphbTRsamRjbXMwMHN0dGcuYXBwcy5nb29nbGV1c2VyY29udGVudC5jb20iLCJzdWIiOiIxMDI1NDczODM3NDQ2MDMxODQ1MDkiLCJlbWFpbCI6ImdidWthdXNrYXNAZ21haWwuY29tIiwiZW1haWxfdmVyaWZpZWQiOnRydWUsIm5iZiI6MTczOTMxMDY0NCwibmFtZSI6IkdlZGltaW5hcyBCdWthdXNrYXMiLCJwaWN0dXJlIjoiaHR0cHM6Ly9saDMuZ29vZ2xldXNlcmNvbnRlbnQuY29tL2EvQUNnOG9jTEk4cE9VbllQY0ZlZzBmcGswMFNWaU95eW9pS3ZwaEFkX1hTWDRlVkVzdk9TNDNVaz1zOTYtYyIsImdpdmVuX25hbWUiOiJHZWRpbWluYXMiLCJmYW1pbHlfbmFtZSI6IkJ1a2F1c2thcyIsImlhdCI6MTczOTMxMDk0NCwiZXhwIjoxNzM5MzE0NTQ0LCJqdGkiOiJjMWFiYTE0NTkxZDkzZTM1MDlmOTU5YWY3YjY4NDU4NDAxY2E0MDMzIn0.DCrHbXu4hZBmhYkcOqsXV8BruHeCI3irvbAbcSC6ZgVF5EGup_pvG4_yD1r4xoZIMEsLDaXgOCOB1KO_MN8Rq7gv_DVO4_afq2MIBB_ZqKyAuP43jQJ5wQyTP-vIEek9MUJDoQv6fIZBCgBP60-PMPNNlJJwh4dHu9LwgTUdCpvo1UScIZIWFySqdW3j-43yOODkxaCXXbldgi2upF-Ve9tuIlqfeTWQ3vG7aVIF5iGNYUaR10VJeKsav02fiBY_7N4emwyR5jkhJuICorFlI7mSQKP8y-ya4jq79ttnKbaZHOGgPSY_dWUwLUgizwRIaKxanJ1_GNWufQ9B6RIAiw