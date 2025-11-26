using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Text;
using WEBtransitions.ClassLibraryDatabase.CustomPager;
using WEBtransitions.ClassLibraryDatabase.DBContext;
using WEBtransitions.CustomErrors;
using WEBtransitions.Services.Interfaces;


namespace WEBtransitions.Services
{
    public class CategorySvc : CommonSvc, IDatabaseSvc<Category, string>, IDisposable       // IPagedCollection<Category>, 
    {
        const int MAX_FILESIZE = 5000 * 1024;

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
        public CategorySvc(IDbContextFactory<NorthwindContext> factory)
        {
            this.factory = factory;
        }

        public IQueryable<Category> GetAllEntities()
        {
            return this.Ctx.Categories;
        }

        public async Task<PgResponse<Category>> GetCurrentPageAsync(StateForComponent currentState)
        {
            Debug.Assert(currentState != null && currentState.PagerState != null);

            PgResponse<Category> currentPage;
            string query = this.PrepareSQL(currentState);
            int totalRecords = await CountRecordsAsync(this.Ctx, query, currentState);
            if (totalRecords < currentState.PagerState.PageSize * (currentState.PagerState.PageNumber - 1))
            {
                currentState.PagerState.PageNumber = 1;
            }

            Category[] allRecords;

            if (totalRecords > 0)
            {
                allRecords = await this.Ctx.Categories.FromSqlRaw(query)
                                        .Skip((currentState.PagerState.PageNumber - 1) * currentState.PagerState.PageSize)
                                        .Take(currentState.PagerState.PageSize)
                                        .ToArrayAsync();
            }
            else
            {
                allRecords = [];
            }
            if (!String.IsNullOrEmpty(currentState.LastInsertedId) && !Array.Exists<Category>(allRecords, x => x.ItemKey == currentState.LastInsertedId))
            {
                var newRecord = await this.Ctx.Categories.Where(x => x.ItemKey == currentState.LastInsertedId && x.IsDeleted == 0).FirstOrDefaultAsync();
                if (newRecord != null)
                {
                    allRecords = allRecords.Append(newRecord).ToArray();
                }
            }

            currentPage = new PgResponse<Category>()
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

        /// <summary>
        /// Builds SQL statement
        /// </summary>
        /// <param name="currentState">Current state</param>
        /// <returns>SQL query without paging</returns>
        internal string PrepareSQL(StateForComponent currentState)
        {
            Debug.Assert(currentState != null);
            StringBuilder bld = new StringBuilder("select * from Categories  WHERE IsDeleted = 0 ");
            if (currentState.FilterState != null && !String.IsNullOrEmpty(currentState.FilterState.Item1))
            {
                if (currentState.FilterState.Item4 || currentState.FilterState.Item5)     // Date or numeric value?
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

        public Task<IEnumerable<Category>> CreateEntities(IEnumerable<Category> collection)
        {
            throw new NotImplementedException();
        }

        public async Task<Category> CreateEntity(Category entity)
        {
            Debug.Assert(entity != null && !entity.CategoryId.HasValue);
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

        public async Task<Category> UpdateEntity(Category entity, bool ignoreConcurrencyError = false)
        {
            Debug.Assert(entity != null && entity.CategoryId.HasValue && entity.CategoryId > 0);
            try
            {
                Category? category = await this.Ctx.Categories.Where(x => x.CategoryId == entity.CategoryId.Value && x.IsDeleted == 0).FirstOrDefaultAsync();
                if (category != null)
                {
                    category.CategoryName = entity.CategoryName;
                    category.Description = entity.Description;
                    category.Picture = entity.Picture;

                    int status = this.Ctx.SaveChanges();
                    if (status < 1)
                    {
                        throw new DbUpdateException();
                    }
                    this.Ctx.Entry(category).Reload();  // Mandatory for SQLite; no need in this operation for MS SQL Server, mySQL and PostgreSQL 
                    return category;
                }
                else
                {
                    throw new NotFoundException($"Category ID={entity.ItemKey} was not found.");
                }
            }
            catch (DbUpdateException ex)
            {
                throw new DatabaseException($"An error occurred while saving the entity changes, ID={entity.ItemKey}", ex);
            }
        }

        public async Task<bool> DeleteEntityByIdAsync(Category entity, bool ignoreConcurrencyError = false)
        {
            Debug.Assert(entity != null && entity.CategoryId.HasValue && entity.CategoryId.Value > 0);
            try
            {
                Category? dbCategory = await this.Ctx.Categories.Where(x => x.CategoryId == entity.CategoryId && x.IsDeleted == 0).FirstOrDefaultAsync();
                if (dbCategory != null)
                {
                    dbCategory.IsDeleted = 1;
                    dbCategory.Version = entity.IgnoreConcurency ? -1 : entity.Version;
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

        public async Task<Category?> GetEntityByIdAsync(string id)
        {
            int categoryId;
            if (!int.TryParse(id, out categoryId))
            {
                throw new InvalidRequestException($"{id} is not integer.");
            }

            return await this.Ctx.Categories.Where(x => x.CategoryId == categoryId && x.IsDeleted == 0).FirstOrDefaultAsync();
        }

        public PgResponse<Category> GetPage(IEnumerable<Category> collection, int pageSize, int pageNumber)
        {
            throw new NotImplementedException();
        }
    }
}
