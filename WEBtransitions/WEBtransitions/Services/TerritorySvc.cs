using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq;
using System.Text;
using WEBtransitions.ClassLibraryDatabase.CustomPager;
using WEBtransitions.ClassLibraryDatabase.DBContext;
using WEBtransitions.CustomErrors;
using WEBtransitions.Services.Interfaces;

namespace WEBtransitions.Services
{
    public class TerritorySvc : CommonSvc, IDatabaseSvc<Territory, string>, IDisposable
    {
        private NorthwindContext? _ctx = null;
        public NorthwindContext Ctx
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
            internal set
            {
                Debug.Assert(value != null);
                _ctx = value;
            }
        }

        public void Dispose()
        {
            if (_ctx != null)
            {
                _ctx.Dispose();
                _ctx = null;
            }
        }

        private IDbContextFactory<NorthwindContext> factory;
        public TerritorySvc(IDbContextFactory<NorthwindContext> factory)
        {
            this.factory = factory;
        }

        public IQueryable<Category> GetAllEntities()
        {
            return this.Ctx.Categories;
        }

        public Task<IEnumerable<Territory>> CreateEntities(IEnumerable<Territory> collection)
        {
            throw new NotImplementedException();
        }

        public async Task<Territory> CreateEntity(Territory entity)
        {
            Debug.Assert(entity != null && !String.IsNullOrEmpty(entity.TerritoryId));
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

        public async Task<bool> DeleteEntityByIdAsync(Territory entity, bool ignoreConcurrencyError = false)
        {
            Debug.Assert(entity != null && !String.IsNullOrEmpty(entity.TerritoryId));
            try
            {
                Territory? dbTerritory = await this.Ctx.Territories.Where(x => x.TerritoryId == entity.TerritoryId && x.IsDeleted == 0).FirstOrDefaultAsync();
                if (dbTerritory != null)
                {
                    dbTerritory.IsDeleted = 1;
                    dbTerritory.Version = entity.IgnoreConcurency ? -1 : entity.Version;
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

        public async Task<Territory?> GetEntityByIdAsync(string id)
        {
            return await this.Ctx.Territories.Where(x => x.TerritoryId == id && x.IsDeleted == 0).FirstOrDefaultAsync();
        }

        public async Task<Territory> UpdateEntity(Territory entity, bool ignoreConcurrencyError = false)
        {
            Debug.Assert(entity != null && !String.IsNullOrEmpty(entity.TerritoryId));
            try
            {
                Territory? territory = await this.Ctx.Territories.Where(x => x.TerritoryId == entity.TerritoryId && x.IsDeleted == 0).FirstOrDefaultAsync();
                if (territory != null)
                {
                    territory.TerritoryDescription = entity.TerritoryDescription;
                    territory.RegionId = entity.RegionId;

                    int status = this.Ctx.SaveChanges();
                    if (status < 1)
                    {
                        throw new DbUpdateException();
                    }
                    this.Ctx.Entry(territory).Reload();  // Mandatory for SQLite; no need in this operation for MS SQL Server, mySQL and PostgreSQL 
                    return territory;
                }
                else
                {
                    throw new NotFoundException($"territory ID={entity.TerritoryId} was not found.");
                }
            }
            catch (DbUpdateException ex)
            {
                throw new DatabaseException($"An error occurred while saving the entity changes, ID={entity.TerritoryId}", ex);
            }
        }

        public async Task<PgResponse<Territory>> GetCurrentPageAsync(StateForComponent currentState)
        {
            Debug.Assert(currentState != null && currentState.PagerState != null);

            PgResponse<Territory> currentPage;
            try
            {
                string query = this.PrepareSQL(currentState);
                int totalRecords = await CountRecordsAsync(this.Ctx, query, currentState);
                if (totalRecords < currentState.PagerState.PageSize * (currentState.PagerState.PageNumber - 1))
                {
                    currentState.PagerState.PageNumber = 1;
                }

                Territory[] allRecords;

                if (totalRecords > 0)
                {
                    allRecords = await this.Ctx.Territories.FromSqlRaw(query)
                                            .Skip((currentState.PagerState.PageNumber - 1) * currentState.PagerState.PageSize)
                                            .Take(currentState.PagerState.PageSize)
                                            .ToArrayAsync();
                }
                else
                {
                    allRecords = [];
                }

                if (!String.IsNullOrEmpty(currentState.LastInsertedId) && !Array.Exists<Territory>(allRecords, x => x.TerritoryId == currentState.LastInsertedId))
                {
                    var newRecord = await this.Ctx.Territories.Where(x => x.TerritoryId == currentState.LastInsertedId && x.IsDeleted == 0).FirstOrDefaultAsync();
                    if (newRecord != null)
                    {
                        allRecords = allRecords.Append(newRecord).ToArray();
                    }
                }
                currentPage = new PgResponse<Territory>()
                {
                    TotalRecords = currentState.PagerState.RowCount,
                    TotalPages = currentState.PagerState.PageCount,
                    PageSize = currentState.PagerState.PageSize,
                    PageNumber = currentState.PagerState.PageNumber,
                    Items = allRecords
                };
                if (currentPage.PageNumber > currentPage.TotalPages)
                {
                    currentPage.PageNumber = 1;
                }

                return currentPage;
            }
            catch (Exception ex)
            {
                string message = ex.Message;
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
            const string sql = "SELECT trt.*, rgn.RegionDescription AS RegionDescription " +
                               "FROM Territories trt " +
                               "LEFT OUTER JOIN Regions rgn ON trt.RegionID = rgn.RegionID " +
                               "WHERE trt.IsDeleted = 0 ";
            Debug.Assert(currentState != null);
            StringBuilder bld = new StringBuilder(sql);
            
            if (currentState.FilterState != null && !String.IsNullOrEmpty(currentState.FilterState.Item1))
            {
                if (currentState.FilterState.Item4 || currentState.FilterState.Item5)     // Date or numeric value?
                {
                    if (!String.IsNullOrEmpty(currentState.FilterState.Item2))
                    {
                        bld.Append(String.Format("AND trt.{0} >= '{1}' ", currentState.FilterState.Item1, currentState.FilterState.Item2));
                    }
                    if (!String.IsNullOrEmpty(currentState.FilterState.Item3))
                    {
                        bld.Append(String.Format("AND trt.{0} <= '{1}' ", currentState.FilterState.Item1, currentState.FilterState.Item3));
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
                string sortPrefix = sortDefinition.Item2 == "RegionID" ? "trt." : "";
                bld.AppendLine($"ORDER BY {sortPrefix}{sortDefinition.Item2} {sortSuffic} ");
            }

            return bld.ToString();
        }

        IQueryable<Territory> IDatabaseSvc<Territory, string>.GetAllEntities()
        {
            throw new NotImplementedException();
        }
    }
}
