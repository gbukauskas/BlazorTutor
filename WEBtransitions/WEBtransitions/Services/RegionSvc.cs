using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using WEBtransitions.ClassLibraryDatabase.DBContext;
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

        public Task<Region> CreateEntity(Region newEntity)
        {
            throw new NotImplementedException();
        }

        public Task<string> DeleteEntityByIdAsync(string id)
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
    }
}
