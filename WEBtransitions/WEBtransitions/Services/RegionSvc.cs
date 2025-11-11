using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using WEBtransitions.ClassLibraryDatabase.DBContext;
using WEBtransitions.CustomErrors;
using WEBtransitions.Services.Interfaces;

namespace WEBtransitions.Services
{
    public class RegionSvc : IDatabaseSvc<Region, string>
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
        }
        private IDbContextFactory<NorthwindContext>? factory = null;

        public RegionSvc(IDbContextFactory<NorthwindContext>? factory)
        {
            this.factory = factory;
        }

        ~RegionSvc()
        {
            if (_ctx != null)
            {
                _ctx.Dispose();
                _ctx = null;
            }
        }

        public Task<IEnumerable<Region>> CreateEntities(IEnumerable<Region> collection)
        {
            throw new NotImplementedException();
        }

        public async Task<Region> CreateEntity(Region entity)
        {
            Debug.Assert(entity != null && !String.IsNullOrEmpty(entity.RegionDescription));
            try
            {
                Region? dbRegion = await this.Ctx.Regions.Where(x => x.RegionDescription == entity.RegionDescription).FirstOrDefaultAsync();
                if (dbRegion == null)
                {
                    Ctx.Add(entity);
                    await Ctx.SaveChangesAsync();
                    return entity;
                }
                else if (dbRegion.IsDeleted == 1)
                {
                    entity.IgnoreConcurency = true;
                    entity.IsDeleted = 0;
                    dbRegion!.AssignProperties(entity);
                    Ctx.Entry(dbRegion).State = EntityState.Modified;
                    await Ctx.SaveChangesAsync();
                    return dbRegion;
                }
                return dbRegion;    // Ignore dublications
            }
            catch (DbUpdateException ex)
            {
                throw new DatabaseException("An error occurred while saving the entity changes.", ex);
            }
        }
        public async Task<Region> CreateEntity(string name)
        {
            var region = new Region() 
            { 
                RegionDescription = name
            };
            var rzlt = await CreateEntity(region);
            return rzlt;
        }

        public Task<bool> DeleteEntityByIdAsync(Region entity, bool ignoreConcurrencyError = false)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Region> GetAllEntities()
        {
            return this.Ctx.Regions;
        }

        public Task<Region?> GetEntityByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<Region> UpdateEntity(Region entity, bool ignoreConcurrencyError)
        {
            throw new NotImplementedException();
        }

        public Tuple<string?, string> SetSort(string? sortParameter, bool setNextState)
        {
            throw new NotImplementedException();
        }
    }
}
