using Microsoft.EntityFrameworkCore;
using openglclevel_server_data.DataAccess;
using openglclevel_server_infrastructure.Repositories.Interfaces;
using openglclevel_server_models.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace openglclevel_server_infrastructure.Repositories
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
    {

        private readonly OpenglclevelContext _dbContext;

        public BaseRepository(OpenglclevelContext dbContext)
        {
            _dbContext = dbContext;
        }

        public virtual async Task AddAsync(TEntity entity)
        {
            await _dbContext.AddAsync(entity);
        }

        public virtual async Task AddRangeAsync(IEnumerable<TEntity> entities)
        {
            await _dbContext.AddRangeAsync(entities);
        }

        public virtual void DeleteAsync(TEntity entity)
        {
            _dbContext.Remove(entity);
        }

        public virtual void DeleteRangeAsync(IEnumerable<TEntity> entities)
        {
            _dbContext.RemoveRange(entities);
        }

        public virtual IQueryable<TEntity> FindByExpresion(Expression<Func<TEntity, bool>> expression)
        {
            return _dbContext.Set<TEntity>().Where(expression);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _dbContext.Set<TEntity>().ToListAsync();
        }

        public async Task<PaginationListEntityModel<TEntity>> GetAllPagedAsync<T>(int page, int pageSize, Expression<Func<TEntity, T>> sorter, IEnumerable<TEntity> filterableEntityQry = null)
        {
            IEnumerable<TEntity> allData = filterableEntityQry == null ? await GetAllAsync() : filterableEntityQry;

            var dataCount = allData.Count();
            var totalPages = Math.Ceiling((decimal)dataCount / pageSize);

            var orderedData = allData.AsQueryable().OrderBy(sorter);

            var pagedQuery = orderedData.Skip(page * pageSize).Take(pageSize).ToList();

            PaginationListEntityModel<T> response = new PaginationListEntityModel<T>();

            return new PaginationListEntityModel<TEntity>()
            {
                PagedList = pagedQuery,
                TotalPages = totalPages,
                PageNumber = page,
                TotalCount = dataCount,
            };

        }


        public virtual async Task<TEntity> GetByIdAsync(Guid id)
        {
            return await _dbContext.Set<TEntity>().FindAsync(id);
        }

        public void UpdateAsync(TEntity entity)
        {
            _dbContext.Update(entity);
        }
    }
}
