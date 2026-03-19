using ClassLibraryDatabase.WEBtransitions_DBContext.Models;
using Microsoft.EntityFrameworkCore;
using WEBtransitions.ClassLibraryDatabase.CustomFilter;

namespace ClassLibraryDatabase.WEBtransitions_DBContext.Models
{
    public partial class TerritoryWithRegion : Territory
    {
        /// <summary>
        /// [dbo].[Region].[RegionDescription]
        /// </summary>
        [AllowFiltering]
        public string? RegionDescription { get; set; }

        internal static void Configure_1(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TerritoryWithRegion>(entity =>
            {
                entity.ToView("TerritoryWithRegion")
                    .HasNoKey();

                entity.Property(e => e.RegionDescription).HasColumnType("TEXT").HasMaxLength(50);
            });
        }

    }
}
