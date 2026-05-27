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
    public class CustomerSvc : CommonSvc, IDatabaseSvc<Customer, string>, IDisposable
    {
        public CustomerSvc(IDbContextFactory<NorthwindContext>? factory): base(factory)
        {
        }

        public IQueryable<Customer> GetAllEntities()
        {
            return this.Ctx.Customers.Where(cust => cust.IsDeleted == 0);
        }

        #region NotImplemented
        public Task<IEnumerable<Customer>> CreateEntities(IEnumerable<Customer> collection)
        {
            throw new NotImplementedException();
        }

        public Task<Customer> CreateEntity(Customer newEntity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteEntityByIdAsync(Customer entity, bool ignoreConcurrencyError = false)
        {
            throw new NotImplementedException();
        }

        public Task<Customer?> GetEntityByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Tuple<string?, string> SetSort(string? sortParameter, bool setNextState)
        {
            throw new NotImplementedException();
        }

        public Task<Customer> UpdateEntity(Customer entity, bool ignoreConcurrencyError = false)
        {
            throw new NotImplementedException();
        }
        #endregion

        public void Dispose()
        {
            if (_ctx != null)
            {
                _ctx.Dispose();
                _ctx = null;
            }
            GC.SuppressFinalize(this);
        }
    }
}
