using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using WEBtransitions.ClassLibraryDatabase.CustomPager;
using WEBtransitions.ClassLibraryDatabase.DBContext;
using WEBtransitions.CustomErrors;
using WEBtransitions.Services.Interfaces;

namespace WEBtransitions.Services
{
    public class CustomerSvc : IDatabaseSvc<Customer, string>, IPagedCollection<Customer>
    {
        /// <summary>
        /// Precision for comparision of double constatns
        /// </summary>
        public static readonly double DELTA = 0.000001;

        public NorthwindContext? ctx = null;
        private IDbContextFactory<NorthwindContext>? factory = null;

        public CustomerSvc(IDbContextFactory<NorthwindContext>? factory)
        {
            this.factory = factory;
        }

        ~CustomerSvc()
        {
            if (ctx != null)
            {
                ctx.Dispose();
                ctx = null;
            }
        }

        public IQueryable<Customer> GetAllEntities(NorthwindContext? ctxNew = null)
        {
            if (ctxNew != null)
            {
                return ctxNew.Customers;
            }
            else if (ctx != null)
            {
                return ctx.Customers;
            }
            else
            {
                this.ctx = factory!.CreateDbContext();
                return this.ctx.Customers;
            }
        }
        public async Task<PgResponse<Customer>> GetPageAsync(IQueryable<Customer> collection, int pageSize, int pageNumber)
        {
            Func<int, int, int, IEnumerable<Customer>, PgResponse<Customer>> buildAnswer =
                delegate (int recordCount, int pageNumber, int totalPages, IEnumerable<Customer> items)
            {
                return new PgResponse<Customer>()
                {
                    TotalRecords = recordCount,
                    TotalPages = totalPages,
                    PageSize = pageSize,
                    PageNumber = pageNumber,
                    Items = items
                };
            };

            Debug.Assert(collection != null && (pageNumber == -1 || pageSize > 0 && pageNumber >= 0));   // pageNumber == 0 returns last page
            try
            {
                int recordsCount = await collection.CountAsync(); // Works in UnitTest1.cs
                //int recordsCount = collection.Count();

                if (recordsCount < 1)
                {
                    return buildAnswer(recordsCount, 0, 0, []);
                }
                else if (pageNumber == -1)
                {
                    return buildAnswer(recordsCount, 1, 1, collection.ToArray());   // return all records
                }
                else
                {
                    double pgCount = (double)recordsCount / (double)pageSize;
                    double integral = Math.Truncate(pgCount);
                    double fractional = Math.Abs(pgCount - integral);
                    int totalPages = fractional <= DELTA ? (int)integral : (int)integral + 1;
                    int currentPage = (pageNumber < 1) ? totalPages
                                                       : pageNumber > totalPages ? 1 : pageNumber;

                    var pageContent = await collection    // Works in UnitTest1.cs
                                                .Skip<Customer>((currentPage - 1) * pageSize)
                                                .Take<Customer>(pageSize)
                                                .ToArrayAsync();
                    //var pageContent = collection
                    //                    .Skip<Customer>((currentPage - 1) * pageSize)
                    //                    .Take<Customer>(pageSize)
                    //                    .ToArray();

                    return buildAnswer(recordsCount, currentPage, totalPages, pageContent);
                }
            }
            catch (Exception ex)
            {
                throw new DatabaseException("GetPage failed.", ex);
            }
        }


        public Task<IEnumerable<Customer>> CreateEntities(NorthwindContext ctx, IEnumerable<Customer> collection)
        {
            throw new NotImplementedException();
        }

        public Task<Customer> CreateEntity(NorthwindContext ctx, Customer newEntity)
        {
            throw new NotImplementedException();
        }

        public Task<string> DeleteEntityByIdAsync(NorthwindContext ctx, string id)
        {
            throw new NotImplementedException();
        }

        public Task<Customer> GetEntityByIdAsync(NorthwindContext ctx, string id)
        {
            throw new NotImplementedException();
        }

        public Task<Customer> UpdateEntity(NorthwindContext ctx, Customer entity)
        {
            throw new NotImplementedException();
        }

    }
}
