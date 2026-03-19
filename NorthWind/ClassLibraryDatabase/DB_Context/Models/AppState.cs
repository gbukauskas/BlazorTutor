using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClassLibraryDatabase.DB_Context.Models
{
    public partial class AppState
    {
        public required string AppName { get; set; }
        public required string UserId { get; set; }
        public required string ComponentName { get; set; }

        public string? SortState { get; set; }
        public string? FilterFieldName { get; set; }
        public string? FilterFieldValue { get; set; }
        public string? FilterFieldMaxValue { get; set; }

        /// <summary>
        /// <list type="bullet">
        ///     <item>1 - date</item>
        ///     <item>2 - integer number</item>
        ///     <item>0 - all other types</item>
        /// </list>
        /// </summary>
        public byte FilterIsDateValue { get; set; }    // Consider converter: https://learn.microsoft.com/en-us/ef/core/modeling/value-conversions?tabs=fluent-api

        public int? PagerButtonCount { get; set; }
        public int? PagerRowCount { get; set; }
        public int? PagerPageCount { get; set; }
        public int? PagerPageNumber { get; set; }
        public int? PagerPageSize { get; set; }

        public required string PagerBaseUrl { get; set; }
        public byte IsDeleted { get; set; }
        public DateTime? DateCreated { get; set; }

        [NotMapped]
        public string? LastInsertedId { get; set; }
        [NotMapped]
        public byte IsNew { get; set; }

        static internal void Configure(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AppState>().Ignore(p => p.IsNew);

            modelBuilder.Entity<AppState>(entity =>
            {
                entity
                    .ToTable("AppStates")
                    .HasKey(e => new { e.AppName, e.UserId, e.ComponentName }).HasName("PK_AppState");

                entity.Property(e => e.AppName).HasColumnName("AppName").IsRequired().HasColumnType("TEXT").HasMaxLength(100);
                entity.Property(e => e.UserId).HasColumnName("UserId").IsRequired().HasColumnType("TEXT").HasMaxLength(200);
                entity.Property(e => e.ComponentName).HasColumnName("ComponentName").HasColumnType("TEXT").HasMaxLength(100);

                entity.Property(e => e.SortState).HasColumnName("SortState").HasColumnType("TEXT").HasMaxLength(2000);
                entity.Property(e => e.FilterFieldName).HasColumnName("FilterFieldName").HasColumnType("TEXT").HasMaxLength(500);
                entity.Property(e => e.FilterFieldValue).HasColumnName("FilterFieldValue").HasColumnType("TEXT").HasMaxLength(2000);
                entity.Property(e => e.FilterFieldMaxValue).HasColumnName("FilterFieldMaxValue").HasColumnType("TEXT").HasMaxLength(2000);
                entity.Property(e => e.FilterIsDateValue).HasColumnName("FilterIsDateValue").IsRequired().HasColumnType("INTEGER");

                entity.Property(e => e.PagerButtonCount).HasColumnName("PagerButtonCount").HasColumnType("INTEGER");
                entity.Property(e => e.PagerRowCount).HasColumnName("PagerRowCount").HasColumnType("INTEGER");
                entity.Property(e => e.PagerPageCount).HasColumnName("PagerPageCount").HasColumnType("INTEGER");
                entity.Property(e => e.PagerPageNumber).HasColumnName("PagerPageNumber").HasColumnType("INTEGER");
                entity.Property(e => e.PagerPageSize).HasColumnName("PagerPageSize").HasColumnType("INTEGER");

                entity.Property(e => e.PagerBaseUrl).HasColumnName("PagerBaseUrl").HasColumnType("TEXT").HasMaxLength(200);
                entity.Property(e => e.IsDeleted).HasColumnType("INTEGER").HasDefaultValue(0);
//                entity.Property(e => e.LastInsertedId).HasColumnName("LastInsertedId").HasColumnType("TEXT").HasMaxLength(500);
                entity.Property(e => e.DateCreated).HasColumnName("DateCreated").HasColumnType("DATE").HasDefaultValueSql("GetUtcDate()");
            });
        }
    }
}
