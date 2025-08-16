using WEBtransitions.ClassLibraryDatabase.CustomPager;

namespace WEBtransitions.Services.Interfaces
{
    public interface IPagedCollection<T> where T : class
    {
        public PgResponse<T> GetPage(IEnumerable<T> collection, int pageSize, int pageNumber);
    }
}
