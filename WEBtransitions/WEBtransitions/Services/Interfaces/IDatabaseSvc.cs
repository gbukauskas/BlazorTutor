using WEBtransitions.ClassLibraryDatabase.DBContext;

namespace WEBtransitions.Services.Interfaces
{
    public interface IDatabaseSvc<T, K> where T : class 
                                        where K : IComparable
    {
        IQueryable<T> GetAllEntities(NorthwindContext? ctxNew = null);
        //IQueryable<T> GetAllEntities(NorthwindContext ctx);

        Task<T> CreateEntity(NorthwindContext ctx, T newEntity);
        Task<IEnumerable<T>> CreateEntities(NorthwindContext ctx, IEnumerable<T> collection);
        Task<T> UpdateEntity(NorthwindContext ctx, T entity);
        Task<T> GetEntityByIdAsync(NorthwindContext ctx, K id);
        Task<K> DeleteEntityByIdAsync(NorthwindContext ctx, K id);

    }
}
