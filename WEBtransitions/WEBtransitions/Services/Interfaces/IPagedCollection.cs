using WEBtransitions.ClassLibraryDatabase.CustomPager;

namespace WEBtransitions.Services.Interfaces
{
    public interface IPagedCollection<T> where T : class
    {
        //public Task<PgResponse<T>> GetPageAsync(IQueryable<T> collection, int pageSize, int pageNumber);
        public PgResponse<T> GetPageAsync(IEnumerable<T> collection, int pageSize, int pageNumber);
    }
}
