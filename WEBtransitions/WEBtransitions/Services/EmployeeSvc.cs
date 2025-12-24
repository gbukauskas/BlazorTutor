using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Text;
using WEBtransitions.ClassLibraryDatabase.CustomPager;
using WEBtransitions.ClassLibraryDatabase.DBContext;
using WEBtransitions.CustomErrors;
using WEBtransitions.Services.Interfaces;

namespace WEBtransitions.Services
{
    public class EmployeeSvc: CommonSvc, IDatabaseSvc<Employee, string>, IDisposable        // IPagedCollection<Employee>, 
    {
        //const int MAX_FILESIZE = 5000 * 1024;

        private NorthwindContext? _ctx = null;
        internal NorthwindContext Ctx
        {
            get
            {
                Debug.Assert(this.factory != null || _ctx != null);
                if (_ctx == null)
                {
                    _ctx = factory!.CreateDbContext();
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

        public void Dispose()
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
        public IEnumerable<SelectableItem> GetSelectableItems(int excludeIds, int? reportsToId)
        {
            var allItems = this.Ctx.Employees.Where(x => x.IsDeleted == 0 && x.EmployeeId != excludeIds);
            foreach (var item in allItems)
            { 
                yield return new SelectableItem(item.ItemKey, item.ItemValue, reportsToId.HasValue && reportsToId == item.EmployeeId); 
            }
        }

        public Task<IEnumerable<Employee>> CreateEntities(IEnumerable<Employee> collection)
        {
            throw new NotImplementedException();
        }

        public async Task<Employee> CreateEntity(Employee entity)
        {
            Debug.Assert(entity != null && !entity.EmployeeId.HasValue);
            try
            {
                Ctx.Add(entity);
                await Ctx.SaveChangesAsync();
                return entity;
            }
            catch (DbUpdateException ex)
            {
                throw new DatabaseException("An error occurred while saving the entity changes.", ex);
            }
        }

        public async Task<bool> DeleteEntityByIdAsync(Employee entity, bool ignoreConcurrencyError = false)
        {
            Debug.Assert(entity != null && entity.EmployeeId.HasValue && entity.EmployeeId.Value > 0);
            try
            {
                Employee? dbEmployee = await this.Ctx.Employees.Where(x => x.EmployeeId == entity.EmployeeId && x.IsDeleted == 0).FirstOrDefaultAsync();
                if (dbEmployee != null)
                {
                    dbEmployee.IsDeleted = 1;
                    dbEmployee.Version = entity.IgnoreConcurency ? -1 : entity.Version;
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

        public async Task<Employee?> GetEntityByIdAsync(string id)
        {
            int employeeId;
            if (!int.TryParse(id, out employeeId))
            {
                throw new InvalidRequestException($"{id} is not integer.");
            }

            return await this.Ctx.Employees.Where(x => x.EmployeeId == employeeId && x.IsDeleted == 0).FirstOrDefaultAsync();
        }

        public async Task<Employee> UpdateEntity(Employee entity, bool ignoreConcurrencyError=false)
        {
            Debug.Assert(entity != null && entity.EmployeeId.HasValue && entity.EmployeeId > 0);
            try
            {
                Employee? employee = await this.Ctx.Employees.Where(x => x.EmployeeId == entity.EmployeeId.Value && x.IsDeleted == 0).FirstOrDefaultAsync();
                if (employee != null)
                {
                    employee.LastName = entity.LastName;
                    employee.FirstName = entity.FirstName;
                    employee.Title = entity.Title;
                    employee.TitleOfCourtesy = entity.TitleOfCourtesy;
                    employee.BirthDate = entity.BirthDate;
                    employee.HireDate = entity.HireDate;
                    employee.Address = entity.Address;
                    employee.City = entity.City;
                    employee.Region = entity.Region;
                    employee.PostalCode = entity.PostalCode;
                    employee.Country = entity.Country;
                    employee.HomePhone = entity.HomePhone;
                    employee.Extension = entity.Extension;
                    employee.Notes = entity.Notes;
                    employee.ReportsTo = entity.ReportsTo;
                    employee.Version = entity.IgnoreConcurency ? -1 : entity.Version;
                    employee.RememberRegion = entity.RememberRegion;
                    employee.Photo = entity.Photo;

                    int status = this.Ctx.SaveChanges();
                    if (status < 1)
                    {
                        throw new DbUpdateException();
                    }
                    this.Ctx.Entry(employee).Reload();  // Mandatory for SQLite; no need in this operation for MS SQL Server, mySQL and PostgreSQL 
                    return employee;
                }
                else
                {
                    throw new NotFoundException($"Employee ID={entity.ItemKey} was not found.");
                }
            }
            catch (DbUpdateException ex)
            {
                throw new DatabaseException($"An error occurred while saving the entity changes, ID={entity.ItemKey}", ex);
            }
        }

        public async Task<PgResponse<Employee>> GetCurrentPageAsync(StateForComponent currentState)
        {
            Debug.Assert(currentState != null && currentState.PagerState != null);

            PgResponse<Employee> currentPage;
            string query = this.PrepareSQL(currentState);
            int totalRecords = await CountRecordsAsync(this.Ctx, query, currentState);
            if (totalRecords < currentState.PagerState.PageSize * (currentState.PagerState.PageNumber - 1))
            {
                currentState.PagerState.PageNumber = 1;
            }

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
            if (!String.IsNullOrEmpty(currentState.LastInsertedId) && !Array.Exists<Employee>(allCustomers, x => x.ItemKey == currentState.LastInsertedId))
            {
                var newCustomer = await this.Ctx.Employees.Where(x => x.ItemKey == currentState.LastInsertedId && x.IsDeleted == 0).FirstOrDefaultAsync();
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
            if (currentState.FilterState != null && !String.IsNullOrEmpty(currentState.FilterState.Item1))
            {
                if (currentState.FilterState.Item4)     // Date value?
                {
                    if (!String.IsNullOrEmpty(currentState.FilterState.Item2))
                    {
                        bld.Append(String.Format("AND {0} >= '{1}' ", currentState.FilterState.Item1, currentState.FilterState.Item2));
                    }
                    if (!String.IsNullOrEmpty(currentState.FilterState.Item3))
                    {
                        bld.Append(String.Format("AND {0} <= '{1}' ", currentState.FilterState.Item1, currentState.FilterState.Item3));
                    }
                }
                else if (!String.IsNullOrEmpty(currentState.FilterState.Item2))
                {
                    bld.AppendLine($"AND {currentState.FilterState.Item1} LIKE '%{currentState.FilterState.Item2}%' "); // Filter using text value
                }
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
