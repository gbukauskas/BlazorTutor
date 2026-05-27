using ClassLibraryDatabase.DB_Context;
using ClassLibraryDatabase.DB_Context.Models;
using ClassLibraryDatabase.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace TestBusinessPart
{
    /// <summary>
    /// Testing Customer's service 
    /// </summary>
    [Collection("DatabaseCollection")]
    public class UnitTestCustomer: IClassFixture<DatabaseFixture>
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
            var allRecords = await svc.GetAllEntities().ToListAsync<Customer>();
            Assert.Equal(93, allRecords.Count());
        }

    }
}
