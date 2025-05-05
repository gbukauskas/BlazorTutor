using System.Diagnostics;
using WEBtransitions.ClassLibraryDatabase.DBContext;

// https://medium.com/bina-nusantara-it-division/a-comprehensive-guide-to-implementing-xunit-tests-in-c-net-b2eea43b48b
// https://xunit.net/docs/shared-context

namespace TestBusinessPart
{
    /// <summary>
    ///   <see cref="https://medium.com/bina-nusantara-it-division/a-comprehensive-guide-to-implementing-xunit-tests-in-c-net-b2eea43b48b"/>
    /// </summary>
    [Collection("DatabaseCollection")]
    public class UnitTest1: IClassFixture<DatabaseFixture>
    {
        private readonly DatabaseFixture _fixture;

        public UnitTest1(DatabaseFixture fixture)
        {
            _fixture = fixture;
        }

        /// <summary>
        /// This test verifies all tables and views.
        /// </summary>
        [Fact]
        public void Test1()
        {
            try
            {
                NorthwindContext ctx = _fixture.DbContext;

                // Views
                var products = ctx.AlphabeticalListOfProducts.Where(e => e.UnitPrice > 50).ToList();
                Assert.Equal(5, products.Count());
                var sales = ctx.CategorySalesFor1997s.Where(e => e.CategorySales > 5000000).ToList();
                Assert.Equal(3, sales.Count());
                var sales97 = ctx.CurrentProductLists.Where(e => e.ProductName.StartsWith("ch")).ToList();
                Assert.Equal(5, sales97.Count());
                var suppliersCount = ctx.CustomerAndSuppliersByCities.Where(e => e.City == "Berlin").Count();
                Assert.Equal(2, suppliersCount);
                var prodinvoices = ctx.Invoices.Where(e => e.CustomerName == "Vins et alcools Chevalier" && e.ShipCity == "Reims").ToList();
                Assert.Equal(137, prodinvoices.Count());
                var orderDtlCount1 = ctx.OrderDetailsExtendeds.Where(e => e.ProductId == 33 && e.ProductName == "Geitost").Count();
                Assert.Equal(7951, orderDtlCount1);
                var subtotals = ctx.OrderSubtotals.Where(e => e.OrderId >= 10250 && e.OrderId <= 10255).Count();
                Assert.Equal(6, subtotals);
                var ordersQry = ctx.OrdersQries.Where(e => e.CustomerId == "VICTE").Count();
                Assert.Equal(188, ordersQry);
                var productDtlCount = ctx.ProductDetailsVs.Where(e => e.ProductName.Contains("Syrup")).Count();
                Assert.Equal(1, productDtlCount);
                var salesCount = ctx.ProductSalesFor1997s.Where(e => e.ProductName.Contains("Sauce")).Count();
                Assert.Equal(2, salesCount);
                var expensiveProducts = ctx.ProductsAboveAveragePrices.Where(e => e.UnitPrice > 60).Count();
                Assert.Equal(5, expensiveProducts);
                var categoryCount = ctx.ProductsByCategories.Where(e => e.CategoryName.Contains("Confections")).Count();
                Assert.Equal(13, categoryCount);
                var quarterlyOrders = ctx.QuarterlyOrders
                    .Where(e => !string.IsNullOrEmpty(e.City) && e.City.Contains("London"))
                    .Count();
                Assert.Equal(6, quarterlyOrders);
                var productSalesByCategory = ctx.SalesByCategories.Where(e => e.CategoryName.Contains("Beverages")).Count();
                Assert.Equal(12, productSalesByCategory);
                var salesAmount = ctx.SalesTotalsByAmounts.Where(e => e.CompanyName.Contains("Save-a-lot Markets")).Count();
                Assert.Equal(18, salesAmount);
                var salesByQuarter = ctx.SummaryOfSalesByQuarters
                    .Where(e => e.ShippedDate >= new DateTime(2016, 07, 15, 0, 0, 0) && e.ShippedDate <= new DateTime(2016, 07, 16, 23, 59, 59)).ToList();
                Assert.Equal(11, salesByQuarter.Count());
                var salesByYear = ctx.SummaryOfSalesByYears
                    .Where(e => e.ShippedDate >= new DateTime(2016, 07, 15, 0, 0, 0) && e.ShippedDate <= new DateTime(2016, 07, 15, 23, 59, 59)).ToList();
                Assert.Equal(5, salesByYear.Count());

                // Tables
                var ctg = ctx.Categories.ToList();
                Assert.Equal(8, ctg.Count());
                var customerCount = ctx.Customers.Where(e => e.City == "México D.F.").ToList();
                Assert.Equal(5, customerCount.Count());
                var customersCount = ctx.CustomerDemographics.Count();
                Assert.Equal(0, customersCount);
                var answer = ctx.Employees.Where(e => e.Country == "USA").ToList();
                Assert.Equal(5, answer.Count());
                var orderCount = ctx.Orders.Where(e => e.CustomerId == "VINET").Count();
                Assert.Equal(158, orderCount);
                var orderDtlCount = ctx.OrderDetails.Where(e => e.ProductId == 65).Count();
                Assert.Equal(7905, orderDtlCount);
                var productCount = ctx.Products.Where(e => e.ProductName.StartsWith("ch")).Count();
                Assert.Equal(6, productCount);
                var regions = ctx.Regions.Where(e => e.RegionId > 2).ToList();
                Assert.Equal(2, regions.Count());
                var shipperCount = ctx.Shippers.ToList();
                Assert.Equal(3, shipperCount.Count());
                var supplrsCount = ctx.Suppliers.Where(e => e.Country == "Germany").ToList();
                Assert.Equal(3, supplrsCount.Count());
                var teritoryCount = ctx.Territories.Where(e => e.TerritoryDescription.StartsWith("B")).ToList();
                Assert.Equal(7, teritoryCount.Count());
                
                Debug.WriteLine("All tests were passed");
            }
            catch (Exception ex)
            {
                string s = ex.Message;
            }
        }
    }
}
