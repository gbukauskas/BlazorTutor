using Microsoft.EntityFrameworkCore;

namespace ClassLibraryDatabase.WEBtransitions_DBContext.Models
{
    public partial class OrderSubtotal
    {
        public int? OrderId { get; set; }

        public double? Subtotal { get; set; }

        internal static void Configure(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OrderSubtotal>(entity =>
            {
                entity
                    .HasNoKey()
                    .ToView("Order Subtotals");

                entity.Property(e => e.OrderId).HasColumnName("OrderID").HasColumnType("INTEGER");
                entity.Property(e => e.Subtotal).HasColumnType("REAL");
            });
        }
    }
}

