using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq;
using System.Text;
using WEBtransitions.ClassLibraryDatabase.CustomPager;
using WEBtransitions.ClassLibraryDatabase.DBContext;
using WEBtransitions.Components.Pages;
using WEBtransitions.CustomErrors;
using WEBtransitions.Services.Interfaces;

namespace WEBtransitions.Services
{
    public class EmployeeSvc: CommonSvc, IDatabaseSvc<Employee, string>, IPagedCollection<Employee>
    {
        private NorthwindContext? _ctx = null;
        internal NorthwindContext Ctx
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
        private IDbContextFactory<NorthwindContext> factory;

        public EmployeeSvc(IDbContextFactory<NorthwindContext> factory)
        {
            this.factory = factory;
        }

        ~EmployeeSvc()
        {
            if (_ctx != null)
            {
                _ctx.Dispose();
                _ctx = null;
            }
        }

        public IQueryable<Employee> GetAllEntities()
        {
            return this.Ctx.Employees;
        }

        public Task<IEnumerable<Employee>> CreateEntities(IEnumerable<Employee> collection)
        {
            throw new NotImplementedException();
        }

        public Task<Employee> CreateEntity(Employee newEntity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteEntityByIdAsync(Employee entity, bool ignoreConcurrencyError = false)
        {
            throw new NotImplementedException();
        }

        public Task<Employee?> GetEntityByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<Employee> UpdateEntity(Employee entity, bool ignoreConcurrencyError=false)
        {
            throw new NotImplementedException();
        }

        // TODO: Remove it
        public PgResponse<Employee> GetPage(IEnumerable<Employee> collection, int pageSize, int pageNumber)
        {
            Debug.Assert(collection != null && (pageNumber == -1 || pageSize > 0 && pageNumber >= 0));   // pageNumber == 0 returns last page
            try
            {
                int recordsCount = collection.Count();

                if (recordsCount < 1)
                {
                    var emptyList = new List<Employee>().ToArray();
                    return new PgResponse<Employee>(recordsCount, pageSize, pageNumber, 0, emptyList);
                }
                else if (pageNumber == -1)
                {
                    return new PgResponse<Employee>(recordsCount, pageSize, 1, 1, collection.ToArray());
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
                                        .Skip<Employee>((currentPage - 1) * pageSize)
                                        .Take<Employee>(pageSize)
                                        .ToArray();

                    return new PgResponse<Employee>(recordsCount, pageSize, pageNumber, totalPages, pageContent);
                }
            }
            catch (Exception ex)
            {
                throw new DatabaseException("GetPage failed.", ex);
            }
        }

        public async Task<PgResponse<Employee>> GetCurrentPageAsync(StateForComponent currentState)
        {
            Debug.Assert(currentState != null && currentState.PagerState != null);

            PgResponse<Employee> currentPage;
            string query = this.PrepareSQL(currentState);
            int totalRecords = await CountRecordsAsync(this.Ctx, query, currentState);
            Employee[] allCustomers;

            if (totalRecords > 0)
            {
                allCustomers = await this.Ctx.Employees.FromSqlRaw(query)
                                        .Skip((currentState.PagerState.PageNumber - 1) * currentState.PagerState.PageSize)
                                        .Take(currentState.PagerState.PageSize)
                                        .ToArrayAsync();
            }
            else
            {
                allCustomers = [];
            }
            if (!String.IsNullOrEmpty(currentState.LastInsertedId) && !Array.Exists<Employee>(allCustomers, x => x.EmployeeIdStr == currentState.LastInsertedId))
            {
                var newCustomer = await this.Ctx.Employees.Where(x => x.EmployeeIdStr == currentState.LastInsertedId && x.IsDeleted == 0).FirstOrDefaultAsync();
                if (newCustomer != null)
                {
                    allCustomers = allCustomers.Append(newCustomer).ToArray();
                }
            }

            currentPage = new PgResponse<Employee>()
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

        /// <summary>
        /// Builds SQL statement
        /// </summary>
        /// <param name="currentState">Current state</param>
        /// <returns>SQL query without paging</returns>
        internal string PrepareSQL(StateForComponent currentState)
        {
            Debug.Assert(currentState != null);
            StringBuilder bld = new StringBuilder("SELECT * FROM Employees WHERE IsDeleted = 0 ");
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
    }
}
