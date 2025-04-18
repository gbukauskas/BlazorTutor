using Microsoft.EntityFrameworkCore;
using WEBtransitions.ClassLibraryDatabase.DBContext;

namespace WEBtransitions.Services
{
    public class DatabaseEmployeeSvc: IDatabaseSvc<Employee, string>
    {
        private NorthwindContext? ctx;
        private IDbContextFactory<NorthwindContext> factory;

        public DatabaseEmployeeSvc(IDbContextFactory<NorthwindContext> factory)
        {
            this.factory = factory;
        }
        ~DatabaseEmployeeSvc()
        {
            if (ctx != null)
            {
                ctx.Dispose();
                ctx = null;
            }
        }

        public IQueryable<Employee> GetAllEntities()
        {
            this.ctx = factory.CreateDbContext();
            return this.ctx.Employees;
        }
    }
}
