using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using RecipeApp.Infrastructure.Context;
using RecipeApp.Shared.Bases;
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

        public async Task<ReturnBase<bool>> AddAsync(T entity)
        {
            try
            {
                if (entity is not null)
                {
                    await _dbSet.AddAsync(entity);
                    await _dbContext.SaveChangesAsync();
                }
                return ReturnBaseHandler.Success(true, "");
            }
            catch (Exception ex)
            {
                return ReturnBaseHandler.Failed<bool>(ex.Message);
            }
        }
        public async Task<ReturnBase<bool>> AddRangeAsync(ICollection<T> entities)
        {
            try
            {
                if (entities is not null)
                {
                    await _dbSet.AddRangeAsync(entities);
                    await _dbContext.SaveChangesAsync();
                }
                return ReturnBaseHandler.Success(true, "");
            }
            catch (Exception ex)
            {
                return ReturnBaseHandler.Failed<bool>(ex.Message);
            }
        }
        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await _dbContext.Database.BeginTransactionAsync();
        }
        public async Task CommitAsync()
        {
            await _dbContext.Database.CommitTransactionAsync();
        }
        public async Task<ReturnBase<bool>> DeleteAsync(int id)
        {
            try
            {
                ReturnBase<T> getEntity = await GetByIdAsync(id);
                if (getEntity.Data is null)
                    return ReturnBaseHandler.Failed<bool>(getEntity.Message);

                _dbSet.Remove(getEntity.Data);
                await _dbContext.SaveChangesAsync();

                return ReturnBaseHandler.Success(true, "");
            }
            catch (Exception ex)
            {
                return ReturnBaseHandler.Failed<bool>(ex.Message);
            }
        }
        public async Task<ReturnBase<bool>> DeleteRangeAsync(ICollection<T> entities)
        {
            try
            {
                if (entities is null)
                    return ReturnBaseHandler.Failed<bool>("Entities is null");

                _dbSet.RemoveRange(entities);
                await _dbContext.SaveChangesAsync();

                return ReturnBaseHandler.Success(true, "");
            }
            catch (Exception ex)
            {
                return ReturnBaseHandler.Failed<bool>(ex.Message);
            }
        }
        public async Task<ReturnBase<T>> GetByIdAsync(int id)
        {
            try
            {
                if (id > 0)
                {
                    T? entity = await _dbSet.FindAsync(id);
                    if (entity is not null)
                        return ReturnBaseHandler.Success(entity, "");

                    return ReturnBaseHandler.Failed<T>("Invalid Id");
                }
                return ReturnBaseHandler.Failed<T>("Invalid Id");
            }
            catch (Exception ex)
            {
                return ReturnBaseHandler.Failed<T>(ex.Message);
            }
        }
        public ReturnBase<IQueryable<T>> GetTableAsTracking()
        {
            try
            {
                return ReturnBaseHandler.Success(_dbSet.AsTracking().AsQueryable(), "");
            }
            catch (Exception ex)
            {
                return ReturnBaseHandler.Failed<IQueryable<T>>(ex.Message);
            }
        }
        public ReturnBase<IQueryable<T>> GetTableNoTracking()
        {
            try
            {
                return ReturnBaseHandler.Success(_dbSet.AsNoTracking().AsQueryable(), "");
            }
            catch (Exception ex)
            {
                return ReturnBaseHandler.Failed<IQueryable<T>>(ex.Message);
            }
        }
        public async Task<ReturnBase<bool>> UpdateAsync(T entity)
        {
            try
            {
                _dbSet.Update(entity);
                await _dbContext.SaveChangesAsync();
                return ReturnBaseHandler.Success(true, "");
            }
            catch (Exception ex)
            {
                return ReturnBaseHandler.Failed<bool>(ex.Message);
            }
        }
        public async Task<ReturnBase<bool>> UpdateRangeAsync(ICollection<T> entities)
        {
            try
            {
                _dbSet.UpdateRange(entities);
                await _dbContext.SaveChangesAsync();
                return ReturnBaseHandler.Success(true, "");
            }
            catch (Exception ex)
            {
                return ReturnBaseHandler.Failed<bool>(ex.Message);
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
        public async Task RollbackAsync()
        {
            try
            {
                await _dbContext.Database.RollbackTransactionAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}