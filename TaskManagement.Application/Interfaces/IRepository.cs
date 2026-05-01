using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Domain.Entities;

namespace TaskManagement.Application.Interfaces
{
    public interface IRepository<TEntity> where TEntity : BaseEntity
    {
        TEntity Add(TEntity entity);
        void Remove(TEntity entity);
        IEnumerable<TEntity> AddRange(List<TEntity> entity);
        void Update(TEntity entity);

        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<IEnumerable<TEntity>> GetAllByAsync(Expression<Func<TEntity, bool>> expression);
        IQueryable<TEntity> GetAllByAsQueryable(Expression<Func<TEntity, bool>> expression);

        Task<TEntity> GetByIdAsync(int id);
        TEntity GetBy(Expression<Func<TEntity, bool>> expression);
        Task<TEntity> GetByAsync(Expression<Func<TEntity, bool>> expression);
        Task<TEntity> GetByAsync(Expression<Func<TEntity, bool>> expression, string include);


        Task<TEntity> GetFirstOrderByAsync(Expression<Func<TEntity, object>> expression);
        Task<TEntity> GetLastOrderByAsync(Expression<Func<TEntity, object>> expression);






        Task<bool> IsExisit(Expression<Func<TEntity, bool>> expression);






        Task<IEnumerable<TEntity>> GetAllWithIncludesAsync(
        Expression<Func<TEntity, bool>> filter,
        params Expression<Func<TEntity, object>>[] includes);

        IQueryable<TEntity> GetAllWithIncludesAsQueryable(
        Expression<Func<TEntity, bool>> filter,
        params Expression<Func<TEntity, object>>[] includes);


        IQueryable<TEntity> GetAllAsQueryable();

        IQueryable<TEntity> GetAllAsQueryableNotDeleted();
        IQueryable<TEntity> GetByAsQueryable(Expression<Func<TEntity, bool>> expression);
    }
}
