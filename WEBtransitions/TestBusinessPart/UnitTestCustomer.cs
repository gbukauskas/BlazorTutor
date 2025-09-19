using Microsoft.EntityFrameworkCore;
using WEBtransitions.ClassLibraryDatabase.DBContext;
using WEBtransitions.Services;

namespace TestBusinessPart
{
    /// <summary>
    /// Testing Customer's service 
    /// </summary>
    [Collection("DatabaseCollection")]
    public class UnitTestCustomer : IClassFixture<DatabaseFixture>
    {
        private readonly DatabaseFixture _fixture;

        public UnitTestCustomer(DatabaseFixture fixture)
        {
            _fixture = fixture;
        }

        /// <summary>
        /// This test reads all records from [dbo].[Customers].
        /// </summary>
        [Fact]
        public async Task Test1()
        {
            NorthwindContext ctxTest = _fixture.DbContext;
            var svc = new CustomerSvc(null);
            svc.SetDbContext(ctxTest);
            var allRecords = await svc.GetAllEntities().ToListAsync();
            Assert.Equal(93, allRecords.Count());
        }

        /// <summary>
        /// Returns second page
        /// </summary>
        [Fact]
        public void Test2()
        {
            NorthwindContext ctxTest = _fixture.DbContext;
            var svc = new CustomerSvc(null);
            svc.SetDbContext(ctxTest);

            var tmp = svc.GetAllEntities();
            var page = svc.GetPage(tmp, 10, 2);

            Assert.NotNull(page?.Items);
            var item = page.Items.First();
            Assert.NotNull(item);
            Assert.Equal("BSBEV", item.CustomerId);
        }

        /// <summary>
        /// Returns all records
        /// </summary>
        [Fact]
        public void Test3()
        {
            NorthwindContext ctxTest = _fixture.DbContext;
            var svc = new CustomerSvc(null);
            svc.SetDbContext(ctxTest);

            var tmp = svc.GetAllEntities();
            var page = svc.GetPage(tmp, 0, -1);

            Assert.NotNull(page?.Items);
            Assert.Equal(93, page.Items.Count());
        }

        /// <summary>
        /// Returns first page (10 records). Collection is sorted by CustomerId descending.
        /// </summary>
        [Fact]
        public void Test4()
        {
            NorthwindContext ctxTest = _fixture.DbContext;
            var svc = new CustomerSvc(null);
            svc.SetDbContext(ctxTest);

            var tmp = svc.GetAllEntities().OrderByDescending(x => x.CustomerId);
            var page = svc.GetPage(tmp, 10, 110);

            Assert.NotNull(page?.Items);
            var item = page.Items.First();
            Assert.NotNull(item);
            Assert.Equal("WOLZA", item.CustomerId);
        }
    }
}