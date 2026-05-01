using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TaskManagement.Application.Interfaces;
using TaskManagement.Domain.Entities;
using TaskManagement.Infrastructure.Context;

namespace TaskManagement.Infrastructure.Repository
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
    {
        private readonly ApplicationDBContext _context;
        public Repository(ApplicationDBContext context)
        {
            _context = context;
        }

        public TEntity Add(TEntity entity)
        {
            _context.Set<TEntity>().Add(entity);
            return entity;
        }


        public IEnumerable<TEntity> AddRange(List<TEntity> entities)
        {
            _context.Set<TEntity>().AddRange(entities);
            return entities;
        }

        public void Update(TEntity entity)
        {
            _context.Update<TEntity>(entity);

        }

        public void Remove(TEntity entity)
        {
            _context.Set<TEntity>().Remove(entity);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _context.Set<TEntity>().ToListAsync();
        }

        public async Task<IEnumerable<TEntity>> GetAllByAsync(Expression<Func<TEntity, bool>> expression)
        {
            return await _context.Set<TEntity>().Where(expression).ToListAsync();
        }

        public IQueryable<TEntity> GetAllByAsQueryable(Expression<Func<TEntity, bool>> expression)
        {
            return _context.Set<TEntity>()
                .Where(expression)
                .Where(x => x.IsDeleted == false)
                .AsQueryable<TEntity>();
        }

        public TEntity GetBy(Expression<Func<TEntity, bool>> expression)
        {
            return _context.Set<TEntity>().FirstOrDefault(expression);
        }
        public async Task<TEntity> GetByAsync(Expression<Func<TEntity, bool>> expression)
        {
            return await _context.Set<TEntity>().FirstOrDefaultAsync(expression);
        }

        public async Task<TEntity> GetByAsync(Expression<Func<TEntity, bool>> expression, string include)
        {
            return await _context.Set<TEntity>()
    .Include(include)
    .FirstOrDefaultAsync(expression);
        }


        public async Task<TEntity> GetByIdAsync(int id)
        {
            return await _context.Set<TEntity>().FindAsync(id);
        }

  
        public async Task<TEntity> GetFirstOrderByAsync(Expression<Func<TEntity, object>> expression)
        {
            return await _context.Set<TEntity>().OrderBy(expression).FirstOrDefaultAsync();
        }

        public async Task<TEntity> GetLastOrderByAsync(Expression<Func<TEntity, object>> expression)
        {
            return await _context.Set<TEntity>().OrderBy(expression).LastOrDefaultAsync();
        }



    





 



        public async Task<bool> IsExisit(Expression<Func<TEntity, bool>> expression)
        {
            return await _context.Set<TEntity>().AnyAsync(expression);
        }


        public async Task<IEnumerable<TEntity>> GetAllWithIncludesAsync(
        Expression<Func<TEntity, bool>> filter,
        params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query = _context.Set<TEntity>().Where(filter);

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return await query.ToListAsync();
        }

        public IQueryable<TEntity> GetAllWithIncludesAsQueryable(
            Expression<Func<TEntity, bool>> filter,
            params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query = _context.Set<TEntity>().Where(filter);

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return query;
        }

        public IQueryable<TEntity> GetAllAsQueryable()
        {
            return _context.Set<TEntity>().Where(x => x.IsDeleted == false).AsQueryable();
        }

        public IQueryable<TEntity> GetAllAsQueryableNotDeleted()
        {
            return _context.Set<TEntity>().AsQueryable();
        }
        public IQueryable<TEntity> GetByAsQueryable(Expression<Func<TEntity, bool>> expression)
        {
            return _context.Set<TEntity>().Where(expression);
        }


    }
}
