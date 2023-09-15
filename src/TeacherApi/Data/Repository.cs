using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TeacherApi.Data.Specifications;

namespace TeacherApi.Data
{
    public class Repository<T> : IRepository<T> where T : EntityBase
    {

        #region Fields

        private readonly AppDbContext _dbContext;

        #endregion Fields

        #region Constructors

        public Repository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        #endregion Constructors

        #region Utilities

        private IQueryable<T> _BuildQueryOnSpecification(
            ISpecification<T> specification)
        {
           IQueryable<T> query = _dbContext.Set<T>().AsQueryable();

            if (specification.Includes.Count != 0) {
                query = specification.Includes.Aggregate(query,
                     (current, include) => current.Include(include));
            }
           
            if(specification.OrderBy != null)
            {
                query = query.OrderBy(specification.OrderBy);
            }

            if(specification.OrderByDescending != null)
            {
                query = query.OrderByDescending(
                    specification.OrderByDescending);
            }

            return query;
        }

        #endregion Utilities

        #region Methods 

        public async Task<T> GetItemAsync(ISpecification<T> specification)
        {
            IQueryable<T> query = _BuildQueryOnSpecification(specification);

            return await query.Where(specification.Criteria).FirstOrDefaultAsync();
        }

        public async Task<List<T>> GetListAsync(ISpecification<T> specification)
        {
            IQueryable<T> query = _BuildQueryOnSpecification(specification);

            return await query.Where(specification.Criteria).ToListAsync();
        }

        public async Task AddAsync(T entity)
        {
            await _dbContext.Set<T>().AddAsync(entity);
        }

        public void Remove(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
        }

        public void Update(T entity)
        {
            _dbContext.Set<T>().Update(entity);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _dbContext.SaveChangesAsync() >= 0);
        }

        #endregion Methods
    }
}
