using Microsoft.EntityFrameworkCore;
using WEBtransitions.ClassLibraryDatabase.DBContext;
using WEBtransitions.Services.Interfaces;

namespace WEBtransitions.Services
{
    public class EmployeeSvc: IDatabaseSvc<Employee, string>
    {
        private NorthwindContext? ctx = null;
        private IDbContextFactory<NorthwindContext> factory;

        public EmployeeSvc(IDbContextFactory<NorthwindContext> factory)
        {
            this.factory = factory;
        }

        ~EmployeeSvc()
        {
            if (ctx != null)
            {
                ctx.Dispose();
                ctx = null;
            }
        }

        public IQueryable<Employee> GetAllEntities(NorthwindContext? ctxNew = null)
        {
            if (ctxNew != null)
            {
                return ctxNew.Employees;
            }
            this.ctx = factory.CreateDbContext();
            return this.ctx.Employees;
        }

        public Task<IEnumerable<Employee>> CreateEntities(NorthwindContext ctx, IEnumerable<Employee> collection)
        {
            throw new NotImplementedException();
        }

        public Task<Employee> CreateEntity(NorthwindContext ctx, Employee newEntity)
        {
            throw new NotImplementedException();
        }

        public Task<string> DeleteEntityByIdAsync(NorthwindContext ctx, string id)
        {
            throw new NotImplementedException();
        }

        public Task<Employee> GetEntityByIdAsync(NorthwindContext ctx, string id)
        {
            throw new NotImplementedException();
        }

        public Task<Employee> UpdateEntity(NorthwindContext ctx, Employee entity)
        {
            throw new NotImplementedException();
        }
    }
}
