using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

namespace WEBtransitions.ClassLibraryDatabase.DBContext
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
        public short FilterIsDateValue { get; set; }    // Consider converter: https://learn.microsoft.com/en-us/ef/core/modeling/value-conversions?tabs=fluent-api

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

    public class AppStateKey: IComparable
    {
        public string? AppName { get; init; }
        public string? UserId { get; init; }
        public string? ComponentName { get; init; }

        public AppStateKey(string? appName = null, string? userId = null, string? componentName = null)
        {
            AppName = appName;
            UserId = userId;
            ComponentName = componentName;
        }

        private int CompareToTyped(AppStateKey other)
        {
            String thisValue = $"{this.AppName ?? " "}_{this.UserId ?? " "}_{this.ComponentName ?? " "}";
            String otherValue = $"{other.AppName ?? " "}_{other.UserId ?? " "}_{other.ComponentName ?? " "}";
            return thisValue.CompareTo(otherValue);
        }

        public int CompareTo(object? obj)
        {
            if (obj == null)
            {
                return 1;
            }
            AppStateKey? otherValue = obj as AppStateKey;
            return (otherValue == null) ? 1 : this.CompareToTyped(otherValue);
        }
    }
}
