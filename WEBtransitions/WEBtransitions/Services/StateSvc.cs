using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq;
using WEBtransitions.ClassLibraryDatabase.DBContext;
using WEBtransitions.CustomErrors;
using WEBtransitions.Services.Interfaces;

namespace WEBtransitions.Services
{
    public class StateSvc : CommonSvc, IDatabaseSvc<AppState, AppStateKey>, IDisposable
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
            set
            {
                Debug.Assert(value != null);
                _ctx = value;
            }
        }
        private IDbContextFactory<NorthwindContext>? factory = null;
        public StateSvc(IDbContextFactory<NorthwindContext>? factory)
        {
            this.factory = factory;
        }
        public void SetDbContext(NorthwindContext ctx)
        {
            this._ctx = ctx;
        }

        public void Dispose()
        {
            if (_ctx != null)
            {
                _ctx.Dispose();
                _ctx = null;
            }
        }

        public async Task<AppState> CreateEntity(AppState newEntity)
        {
            try
            {
                Debug.Assert(newEntity != null && !String.IsNullOrEmpty(newEntity.AppName) && !String.IsNullOrEmpty(newEntity.UserId) && !String.IsNullOrEmpty(newEntity.ComponentName));
                var dbState = await Ctx.AppStates.FindAsync(newEntity.AppName, newEntity.UserId, newEntity.ComponentName);
                if (dbState != null)
                {
                    dbState.SortState = newEntity.SortState;
                    dbState.FilterFieldName = newEntity.FilterFieldName;
                    dbState.FilterFieldValue = newEntity.FilterFieldValue;
                    dbState.FilterFieldMaxValue = newEntity.FilterFieldMaxValue;
                    dbState.FilterIsDateValue = newEntity.FilterIsDateValue;

                    dbState.PagerButtonCount = newEntity.PagerButtonCount;
                    dbState.PagerRowCount = newEntity.PagerRowCount;
                    dbState.PagerPageCount = newEntity.PagerPageCount;
                    dbState.PagerPageNumber = newEntity.PagerPageNumber;
                    dbState.PagerPageSize = newEntity.PagerPageSize;

                    dbState.PagerBaseUrl = newEntity.PagerBaseUrl;
                    dbState.IsDeleted = 0;  // Restore the record when it is deleted

                    await Ctx.SaveChangesAsync();
                    dbState.IsNew = 0;
                    return dbState;
                }
                else
                {
                    Ctx.AppStates.Add(newEntity);
                    await Ctx.SaveChangesAsync();
                    newEntity.IsNew = 1;
                    return newEntity;
                }
            }
            catch (Exception ex)
            {
                throw new DatabaseException("New state was not created", ex);
            }
        }

        public async Task<int> CountRecords(AppStateKey key)
        {
            try
            {
                return await Ctx.AppStates.CountAsync(t => t.AppName == key.AppName && t.UserId == key.UserId && t.ComponentName == key.ComponentName);
            }
            catch (Exception ex)
            {
                throw new DatabaseException("Count records failed", ex);
            }
        }

        public async Task<AppState?> GetEntityByIdAsync(AppStateKey id)
        {
            try
            {
                return await Ctx.AppStates.FindAsync(id.AppName, id.UserId, id.ComponentName);
            }
            catch (Exception ex)
            {
                throw new DatabaseException($"GetEntityById failed: AppName='{id.AppName}', UserId='{id.UserId}', ComponentName='{id.ComponentName}", ex);
            }
        }
        /// <summary>
        /// Every user has individual record in dbo.AppStates. Concurrency error would never occur.
        /// </summary>
        /// <param name="entity">Current record</param>
        /// <param name="ignoreConcurrencyError">The application ignores this parameter</param>
        /// <returns>Saved record in the dbo.AppStates table</returns>
        public async Task<AppState> UpdateEntity(AppState entity, bool ignoreConcurrencyError = false)
        {
            AppStateKey key = new AppStateKey(entity.AppName, entity.UserId, entity.ComponentName);
            try
            {
                var ComponentState = await (
                        from s in Ctx.AppStates
                        where s.AppName == entity.AppName && s.UserId == entity.UserId && s.ComponentName == entity.ComponentName
                        select s
                    ).SingleOrDefaultAsync();
                Debug.Assert(ComponentState != null);

                ComponentState.SortState = entity.SortState;

                ComponentState.FilterFieldName = entity.FilterFieldName;
                ComponentState.FilterFieldValue = entity.FilterFieldValue;
                ComponentState.FilterFieldMaxValue = entity.FilterFieldMaxValue;
                ComponentState.FilterIsDateValue = entity.FilterIsDateValue;

                ComponentState.PagerButtonCount = entity.PagerButtonCount;
                ComponentState.PagerRowCount = entity.PagerRowCount;
                ComponentState.PagerPageCount = entity.PagerPageCount;
                ComponentState.PagerPageNumber = entity.PagerPageNumber;
                ComponentState.PagerPageSize = entity.PagerPageSize;
                ComponentState.PagerBaseUrl = entity.PagerBaseUrl;

                ComponentState.IsDeleted = entity.IsDeleted;
                ComponentState.LastInsertedId = entity.LastInsertedId;

                await Ctx.SaveChangesAsync();
                return ComponentState;
            }
            catch (Exception ex)
            { 
                throw new DataBaseUpdateException("The state for 'Customers' was not updated.", ex);
            }
        }

        /* ****************** NOT implemented ****************** */
        public Task<IEnumerable<AppState>> CreateEntities(IEnumerable<AppState> collection)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteEntityByIdAsync(AppState entity, bool ignoreConcurrencyError = false)
        {
            throw new NotImplementedException();
        }

        public IQueryable<AppState> GetAllEntities()
        {
            throw new NotImplementedException();
        }

    }
}
