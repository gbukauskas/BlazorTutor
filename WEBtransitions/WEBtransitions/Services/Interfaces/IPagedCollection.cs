using WEBtransitions.ClassLibraryDatabase.CustomPager;

namespace WEBtransitions.Services.Interfaces
{
    // TODO: Is it used?
    public interface IPagedCollection<T> where T : class
    {
        public PgResponse<T> GetPage(IEnumerable<T> collection, int pageSize, int pageNumber);
    }
}
