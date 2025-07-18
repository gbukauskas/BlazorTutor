﻿using Azure;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using WEBtransitions.ClassLibraryDatabase.CustomPager;
using WEBtransitions.ClassLibraryDatabase.DBContext;
using WEBtransitions.CustomErrors;
using WEBtransitions.Services.Interfaces;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace WEBtransitions.Services
{
    public class CustomerSvc : IDatabaseSvc<Customer, string>, IPagedCollection<Customer>
    {
        /// <summary>
        /// Precision for comparision of double constatns
        /// </summary>
        public static readonly double DELTA = 0.000001;

        private NorthwindContext? _ctx = null;
        public NorthwindContext Ctx
        {
            get
            {
                Debug.Assert(this.factory != null);
                if (_ctx == null)
                {
                    _ctx = factory.CreateDbContext();
                }
                return _ctx!;
            }
            set
            {
                Debug.Assert(value != null);
                _ctx = value;
            }
        }
        private IDbContextFactory<NorthwindContext>? factory = null;

        public CustomerSvc(IDbContextFactory<NorthwindContext>? factory)
        {
            this.factory = factory;
        }

        ~CustomerSvc()
        {
            if (_ctx != null)
            {
                _ctx.Dispose();
                _ctx = null;
            }
        }

        public async Task<bool> IsDublicateKey(string key)
        {
            Customer? oldCustomer = await this.Ctx.Customers.FindAsync(key);
            return oldCustomer != null && oldCustomer.IsDeleted == 0;
        }

        public IQueryable<Customer> GetAllEntities()
        {
            return this.Ctx.Customers.Where(cust => cust.IsDeleted == 0);
        }

        public async Task<PgResponse<Customer>> GetCurrentPageAsync(StateForComponent currentState)
        {
            Debug.Assert(currentState != null && currentState.PagerState != null);

            PgResponse<Customer> currentPage;
            string query = this.PrepareSQL(currentState);
            int totalRecords = await CountRecordsAsync(query, currentState);
            Customer[] allCustomers;

            if (totalRecords > 0)
            {
                allCustomers = await this.Ctx.Customers.FromSqlRaw(query)
                                        .Skip((currentState.PagerState.PageNumber - 1) * currentState.PagerState.PageSize)
                                        .Take(currentState.PagerState.PageSize)
                                        .ToArrayAsync();
            }
            else
            {
                allCustomers = [];
            }
            if (!String.IsNullOrEmpty(currentState.LastInsertedId) && !Array.Exists<Customer>(allCustomers, x => x.CustomerId == currentState.LastInsertedId))
            {
                var newCustomer = await this.Ctx.Customers.Where(x => x.CustomerId == currentState.LastInsertedId && x.IsDeleted == 0).FirstOrDefaultAsync();
                if (newCustomer != null)
                {
                    allCustomers = allCustomers.Append(newCustomer).ToArray();
                }
            }

            currentPage = new PgResponse<Customer>()
            {
                TotalRecords = currentState.PagerState.RowCount,
                TotalPages = currentState.PagerState.PageCount,
                PageSize = currentState.PagerState.PageSize,
                PageNumber = currentState.PagerState.PageNumber,
                Items = allCustomers
            };
            if (currentPage.PageNumber > currentPage.TotalPages)
            {
                currentPage.PageNumber = 1;
            }

            return currentPage;
        }

        private async Task<int> CountRecordsAsync(string query, StateForComponent currentState)
        {
            Debug.Assert(currentState != null && currentState.PagerState != null);
            try
            {
                string countQuery = query.Replace("*", "COUNT(1) AS Value");
                currentState.PagerState.RowCount = await this.Ctx.Database.SqlQueryRaw<int>(countQuery).FirstOrDefaultAsync();

                int pgCount = currentState.PagerState.RowCount / currentState.PagerState.PageSize;
                if (currentState.PagerState.RowCount % currentState.PagerState.PageSize > 0)
                {
                    pgCount += 1;
                }
                currentState.PagerState.PageCount = pgCount;
                return currentState.PagerState.RowCount;
            }
            catch (Exception ex)
            {
                string s = ex.Message;
                throw;
            }
        }

        /// <summary>
        /// Builds SQL statement
        /// </summary>
        /// <param name="currentState">Current state</param>
        /// <returns>SQL query without paging</returns>
        internal string PrepareSQL(StateForComponent currentState)
        {
            Debug.Assert(currentState != null);
            StringBuilder bld = new StringBuilder("SELECT * FROM Customers WHERE IsDeleted = 0 ");
            if (currentState.FilterState != null && !String.IsNullOrEmpty(currentState.FilterState.Item2))
            {
                bld.AppendLine($"AND {currentState.FilterState.Item1} LIKE '%{currentState.FilterState.Item2}%' ");
            }
            if (!String.IsNullOrEmpty(currentState.SortState) && !currentState.SortState.StartsWith("n"))
            {
                Tuple<string?, string> sortDefinition = SetSort(currentState.SortState, false);
                string sortSuffic = sortDefinition.Item1 == "a" ? "ASC" : "DESC";
                bld.AppendLine($"ORDER BY {sortDefinition.Item2} {sortSuffic} ");
            }

            return bld.ToString();
        }

        /// <summary>
        /// <list type="number">
        ///     <item>sortDefinionOriginal is null or empty - no sort</item>
        ///     <item>sortDefinionOriginal starts with "n" - no sort</item>
        ///     <item>sortDefinionOriginal starts with "a" - sort ascending; name of sorting field starts from SortName[2]</item>
        ///     <item>sortDefinionOriginal starts with "d" - sort descending; name of sorting field starts from SortName[2]</item>
        /// </list>
        /// </summary>
        public Tuple<string?, string> SetSort(string? sortParameter, bool setNextState)
        {
            string sortDirection;
            if (String.IsNullOrEmpty(sortParameter))
            {
                return new Tuple<string?, string>(null, "");
            }

            if (setNextState)
            {
                switch (sortParameter.Substring(0, 1))
                {
                    case "n":
                        sortDirection = "a";
                        break;
                    case "a":
                        sortDirection = "d";
                        break;
                    default:
                        sortDirection = "n";
                        break;
                }
            }
            else
            {
                sortDirection = sortParameter.Substring(0, 1);
            }
            string sortName = sortParameter.Substring(2);
            return new Tuple<string?, string>(sortDirection, sortName);
        }

        public PgResponse<Customer> GetPageAsync(IEnumerable<Customer>? collection, int pageSize, int pageNumber)
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
                int recordsCount = collection.Count();

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

                    var pageContent = collection
                                        .Skip<Customer>((currentPage - 1) * pageSize)
                                        .Take<Customer>(pageSize)
                                        .ToArray();

                    return buildAnswer(recordsCount, currentPage, totalPages, pageContent);
                }
            }
            catch (Exception ex)
            {
                throw new DatabaseException("GetPage failed.", ex);
            }
        }


        public Task<IEnumerable<Customer>> CreateEntities(IEnumerable<Customer> collection)
        {
            throw new NotImplementedException();
        }

        public async Task<Customer> CreateEntity(Customer entity)
        {
            Debug.Assert(entity != null && !String.IsNullOrEmpty(entity.CustomerId));
            try
            {
                Customer? dbCustomer = await this.Ctx.Customers.Where(x => x.CustomerId == entity.CustomerId).FirstOrDefaultAsync();
                
                if (dbCustomer == null)
                {
                    Ctx.Add(entity);
                    await Ctx.SaveChangesAsync();
                    return entity;
                }
                else if (dbCustomer.IsDeleted == 1)
                {
                    entity.IgnoreConcurency = true;
                    entity.IsDeleted = 0;
                    dbCustomer!.AssignProperties(entity);
                    Ctx.Entry(dbCustomer).State = EntityState.Modified;
                    await Ctx.SaveChangesAsync();
                    return dbCustomer;
                } 
                else
                {
                    throw new DbUpdateException($"Key {entity.CustomerId} is already used");
                }
            }
            catch (DbUpdateException ex)
            {
                throw new DatabaseException("An error occurred while saving the entity changes.", ex);
            }
        }

        /// <summary>
        /// Returns Customer entity.
        /// </summary>
        /// <param name="id">Customer ID</param>
        /// <param name="ctxNew">Alternative DB context. It allows to call service from another context (Orders for example) </param>
        /// <returns>Customer or null.</returns>
        public async Task<Customer?> GetEntityByIdAsync(string id)
        {
            return await this.Ctx.Customers.Where(x => x.CustomerId == id && x.IsDeleted == 0).FirstOrDefaultAsync();
        }

        public async Task<Customer> UpdateEntity(Customer entity, bool ignoreConcurrencyError = false)
        {
            Debug.Assert(entity != null && !String.IsNullOrEmpty(entity.CustomerId));
            try
            {
                Customer? dbCustomer = await this.Ctx.Customers.Where(x => x.CustomerId == entity.CustomerId && x.IsDeleted == 0).FirstOrDefaultAsync();
                if (dbCustomer != null)
                {
                    dbCustomer.CompanyName = entity.CompanyName;
                    dbCustomer.ContactName = entity.ContactName;
                    dbCustomer.ContactTitle = entity.ContactTitle;
                    dbCustomer.Address = entity.Address;
                    dbCustomer.City = entity.City;
                    dbCustomer.Region = entity.Region;
                    dbCustomer.PostalCode = entity.PostalCode;
                    dbCustomer.Country = entity.Country;
                    dbCustomer.Phone = entity.Phone;
                    dbCustomer.Fax = entity.Fax;
                    dbCustomer.IsDeleted = entity.IsDeleted;

                    dbCustomer.Version = dbCustomer.IgnoreConcurency 
                        ? -1    // Ignore concurrency error
                        : entity.Version;

                    int status = this.Ctx.SaveChanges();
                    return dbCustomer;
                }
                else
                {
                    return entity;
                }
            }
            catch (DbUpdateException ex)
            {
                throw new DatabaseException("An error occurred while saving the entity changes.", ex);
            }
        }

        // https://www.sqlitetutorial.net/sqlite-trigger/
        public async Task<bool> DeleteEntityByIdAsync(Customer entity, bool ignoreConcurrencyError = false)
        {
            Debug.Assert(entity != null && !String.IsNullOrEmpty(entity.CustomerId));
            try
            {
                Customer? dbCustomer = await this.Ctx.Customers.Where(x => x.CustomerId == entity.CustomerId && x.IsDeleted == 0).FirstOrDefaultAsync();
                if (dbCustomer != null)
                {
                    dbCustomer.IsDeleted = 1;
                    dbCustomer.Version = dbCustomer.IgnoreConcurency ? -1 : entity.Version;
                    this.Ctx.SaveChanges();
                    return true;
                }
                else
                { 
                    return false; 
                }
            }
            catch (DbUpdateException ex)
            {
                throw new DatabaseException("An error occurred while removing the entity.", ex);
            }
        }

    }
}
