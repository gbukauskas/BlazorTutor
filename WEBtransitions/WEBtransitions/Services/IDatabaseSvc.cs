using WEBtransitions.ClassLibraryDatabase.DBContext;

namespace WEBtransitions.Services
{
    public interface IDatabaseSvc<T, K> where T : class where K : IComparable
    {
        IQueryable<T> GetAllEntities();
    }
}
