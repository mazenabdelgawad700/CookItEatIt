using Microsoft.EntityFrameworkCore.Storage;

namespace RecipeApp.Infrastructure.InfrastructureBases
{
    public interface IGenericRepositoryAsync<T> where T : class
    {
        Task AddAsync(T entity);
        Task AddRangeAsync(ICollection<T> entities);
        Task UpdateAsync(T entity);
        Task UpdateRangeAsync(ICollection<T> entities);
        Task DeleteAsync(int id);
        Task DeleteRangeAsync(ICollection<T> entities);
        Task<T> GetByIdAsync(int id);
        Task SaveChangesAsync();
        IDbContextTransaction BeginTransaction();
        IQueryable<T> GetTableAsTracking();
        IQueryable<T> GetTableNoTracking();
        void Commit();
        void Rollback();
    }
}
