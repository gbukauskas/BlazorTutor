using ClassLibraryDatabase.DB_Context;
using ClassLibraryDatabase.DB_Context.Models;
using ClassLibraryDatabase.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace ClassLibraryDatabase.Services
{
    public class TerritorySvc : CommonSvc, IDatabaseSvc<Territory, string>, IDisposable
    {
        public TerritorySvc(IDbContextFactory<NorthwindContext>? factory): base(factory)
        {
        }

        public void Dispose()
        {
            if (this._ctx != null)
            {
                this._ctx.Dispose();
                this._ctx = null;
            }
            GC.SuppressFinalize(this);
        }

        public IQueryable<Territory> GetAllEntities()
        {
            return this.Ctx.Territories.Where(x => x.IsDeleted == 0).AsQueryable();
        }
        public IQueryable<TerritoryWithRegion> GetAllEntitiesWithRegion()
        {
            return this.Ctx.TerritoriesWithRegion.Where(x => x.IsDeleted == 0).AsQueryable();
        }



        #region Not_Implemented
        public Task<IEnumerable<Territory>> CreateEntities(IEnumerable<Territory> collection)
        {
            throw new NotImplementedException();
        }

        public Task<Territory> CreateEntity(Territory newEntity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteEntityByIdAsync(Territory entity, bool ignoreConcurrencyError = false)
        {
            throw new NotImplementedException();
        }


        public Task<Territory?> GetEntityByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Tuple<string?, string> SetSort(string? sortParameter, bool setNextState)
        {
            throw new NotImplementedException();
        }

        public Task<Territory> UpdateEntity(Territory entity, bool ignoreConcurrencyError = false)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
