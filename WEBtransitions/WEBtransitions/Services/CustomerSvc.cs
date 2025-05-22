using Azure;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
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

        public PgResponse<Customer> GetCurrentPage_ADO(StateForComponent currentState, string connString)
        {
            Debug.Assert(currentState != null && currentState.PagerState != null);

            PgResponse<Customer> currentPage;
            string query = this.PrepareSQL(currentState);
            using (SqliteConnection conn = new SqliteConnection(connString))
            {
                conn.Open();
                PreparePageValues(conn, query, currentState.PagerState);

                using (SqliteCommand readCommand = new SqliteCommand(query, conn))
                using (SqliteDataReader rdr = readCommand.ExecuteReader())
                {
                    var allCustomers = Customer.LoadFromDB(rdr)
                                        .Skip((currentState.PagerState.PageNumber - 1) * currentState.PagerState.PageSize)
                                        .Take(currentState.PagerState.PageSize)
                                        .ToArray();
                    currentPage = new PgResponse<Customer>()
                    {
                        TotalRecords = currentState.PagerState.RowCount,
                        TotalPages = currentState.PagerState.PageCount,
                        PageSize = currentState.PagerState.PageSize,
                        PageNumber = currentState.PagerState.PageNumber,
                        Items = allCustomers
                    };
                }
                conn.Close();
            }
            return currentPage;
        }

        /// <summary>
        /// Count pages
        /// </summary>
        /// <param name="conn">Current connection</param>
        /// <param name="query">SQL query</param>
        /// <param name="pagerData"><see cref="currentState.PagerState"/></param>
        private void PreparePageValues(SqliteConnection conn, string query, PgPostData pagerData)
        {
            using (SqliteCommand command = new SqliteCommand(query.Replace("*", "COUNT(1)"), conn))
            {
                var rowCountObject = command.ExecuteScalar();
                long rowCont = (long)(rowCountObject == null ? 0L : rowCountObject);
                pagerData.RowCount = (int)rowCont;

                int pgCount = pagerData.RowCount / pagerData.PageSize;
                if (pagerData.RowCount % pagerData.PageSize > 0)
                {
                    pgCount += 1;
                }
                pagerData.PageCount = pgCount;

                if (pagerData.PageNumber * pagerData.PageSize >= rowCont)
                {
                    pagerData.PageNumber = 1;
                }
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
            StringBuilder bld = new StringBuilder("SELECT * FROM Customers ");
            if (currentState.FilterState != null && !String.IsNullOrEmpty(currentState.FilterState.Item2))
            {
                bld.AppendLine($"WHERE {currentState.FilterState.Item1} LIKE '%{currentState.FilterState.Item2}%' ");
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

        public async Task<PgResponse<Customer>> GetPageAsync(IEnumerable<Customer>? collection, int pageSize, int pageNumber)
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
                int recordsCount = collection.Count(); // Works in UnitTest1.cs
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
