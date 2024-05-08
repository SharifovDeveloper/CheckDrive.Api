using CheckDriver.Domain.Common;

namespace CheckDriver.Domain.Interfaces.Repositories
{
    public interface IRepositoryBase<T> where T : EntityBase
    {
        Task<IEnumerable<T>> FindAllAsync();
        Task<T> FindByIdAsync(int id);
        Task<T> CreateAsync(T entity);
        Task UpdateAsync(T entity); 
        Task DeleteAsync(int id);
    }
}
