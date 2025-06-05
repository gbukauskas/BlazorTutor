using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using WEBtransitions.ClassLibraryDatabase.DBContext;
using WEBtransitions.Services.Interfaces;

namespace WEBtransitions.Services
{
    public class EmployeeSvc: IDatabaseSvc<Employee, string>
    {
        private NorthwindContext? _ctx = null;
        internal NorthwindContext Ctx
        {
            get
            {
                Debug.Assert(this.factory != null);
                if (_ctx == null)
                {
                    factory.CreateDbContext();
                }
                return _ctx!;
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
    }
}
