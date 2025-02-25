﻿using Microsoft.EntityFrameworkCore.Storage;
using RecipeApp.Shared.Bases;

namespace RecipeApp.Infrastructure.InfrastructureBases
{
    public interface IGenericRepositoryAsync<T> where T : class
    {
        Task<ReturnBase<bool>> AddAsync(T entity);
        Task<ReturnBase<bool>> AddRangeAsync(ICollection<T> entities);
        Task<ReturnBase<bool>> UpdateAsync(T entity);
        Task<ReturnBase<bool>> UpdateRangeAsync(ICollection<T> entities);
        Task<ReturnBase<bool>> DeleteAsync(int id);
        Task<ReturnBase<bool>> DeleteRangeAsync(ICollection<T> entities);
        Task<ReturnBase<T>> GetByIdAsync(int id);
        ReturnBase<IQueryable<T>> GetTableAsTracking();
        ReturnBase<IQueryable<T>> GetTableNoTracking();
        Task<IDbContextTransaction> BeginTransactionAsync();
        Task SaveChangesAsync();
        Task CommitAsync();
        Task RollbackAsync();
    }
}
