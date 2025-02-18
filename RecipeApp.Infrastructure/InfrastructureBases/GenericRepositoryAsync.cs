using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using RecipeApp.Infrastructure.Context;
namespace RecipeApp.Infrastructure.InfrastructureBases
{
    public class GenericRepositoryAsync<T> : IGenericRepositoryAsync<T> where T : class
    {
        private readonly AppDbContext _dbContext;
        private readonly DbSet<T> _dbSet;

        public GenericRepositoryAsync(AppDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<T>();
        }

        public async Task AddAsync(T entity)
        {
            try
            {
                if (entity is not null)
                {
                    await _dbSet.AddAsync(entity);
                    await _dbContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task AddRangeAsync(ICollection<T> entities)
        {
            try
            {
                if (entities is not null)
                {
                    await _dbSet.AddRangeAsync(entities);
                    await _dbContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public IDbContextTransaction BeginTransaction()
        {
            return _dbContext.Database.BeginTransaction();
        }
        public void Commit()
        {
            _dbContext.Database.CommitTransaction();
        }
        public async Task DeleteAsync(int id)
        {
            try
            {
                var entity = await GetByIdAsync(id);
                _dbSet.Remove(entity);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task DeleteRangeAsync(ICollection<T> entities)
        {
            try
            {
                _dbSet.RemoveRange(entities);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<T> GetByIdAsync(int id)
        {
            try
            {
                if (id > 0)
                {
                    var entity = await _dbSet.FindAsync(id);
                    if (entity is not null)
                        return entity;
                    throw new Exception("Invalid Id");
                }
                throw new Exception("Invalid Id");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public IQueryable<T> GetTableAsTracking()
        {
            try
            {
                return _dbSet.AsTracking().AsQueryable();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public IQueryable<T> GetTableNoTracking()
        {
            try
            {
                return _dbSet.AsNoTracking().AsQueryable();
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}", ex);
            }
        }
        public async Task UpdateAsync(T entity)
        {
            try
            {
                _dbSet.Update(entity);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task UpdateRangeAsync(ICollection<T> entities)
        {
            try
            {
                _dbSet.UpdateRange(entities);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task SaveChangesAsync()
        {
            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public void Rollback()
        {
            try
            {
                _dbContext.Database.RollbackTransaction();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}
