using ClassLibraryDatabase.CustomFilter;
using ClassLibraryDatabase.DB_Context.Models;
using Microsoft.EntityFrameworkCore;

namespace ClassLibraryDatabase.DB_Context.Models
{
    public partial class TerritoryWithRegion

    {
        /// <summary>
        /// [dbo].[Region].[RegionDescription]
        /// </summary>
        [AllowFiltering]
        public string? RegionDescription { get; set; }

        /* ************** Inherited from Territory ************** */
        [AllowFiltering]
        public string? TerritoryId { get; set; }

        [AllowFiltering]
        public string? TerritoryDescription { get; set; }

        [AllowFiltering]
        public int RegionId { get; set; }

        public byte IsDeleted { get; set; }
        public bool IgnoreConcurency { get; set; } = false;
        public int Version { get; set; }

        public virtual Region Region { get; set; } = null!;
        public bool RememberRegion { get; set; } = false;


        internal static void Configure(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TerritoryWithRegion>(entity =>
            {
                entity.ToView("TerritoryWithRegion")
                    .HasNoKey();

                entity.Property(e => e.RegionDescription).HasColumnType("TEXT").HasMaxLength(50);

                entity.Ignore(t => t.IgnoreConcurency);
                entity.Ignore(t => t.RememberRegion);

                entity.Property(e => e.TerritoryId).HasColumnName("TerritoryID").HasColumnType("TEXT").HasMaxLength(20);
                entity.Property(e => e.TerritoryDescription).HasColumnType("TEXT").HasMaxLength(50);
                entity.Property(e => e.RegionId).HasColumnName("RegionID").HasColumnType("INTEGER");

                entity.Property(e => e.IsDeleted).HasColumnType("INTEGER").HasDefaultValue(0);
                entity.Property(e => e.Version).HasColumnType("INTEGER").HasDefaultValue(0).IsRowVersion();

            });
        }

    }
}
