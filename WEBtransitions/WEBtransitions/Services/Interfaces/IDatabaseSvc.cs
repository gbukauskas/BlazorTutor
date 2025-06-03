using WEBtransitions.ClassLibraryDatabase.DBContext;

namespace WEBtransitions.Services.Interfaces
{
    public interface IDatabaseSvc<T, K> where T : class 
                                        where K : IComparable
    {
        IQueryable<T> GetAllEntities();

        Task<T> CreateEntity(T newEntity);
        Task<IEnumerable<T>> CreateEntities(IEnumerable<T> collection);
        Task<T> UpdateEntity(T entity, bool ignoreConcurrencyError);
        Task<T?> GetEntityByIdAsync(K id);
        Task<K> DeleteEntityByIdAsync(K id);

    }
}
