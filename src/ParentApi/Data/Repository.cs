using Microsoft.EntityFrameworkCore;
using ParentApi.Data.Specifications;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParentApi.Data
{
    /// <summary>
    /// Реализация репозитория.
    /// </summary>
    /// <typeparam name="T">Обощенный тип сущности БД.</typeparam>
    public class Repository<T> : IRepository<T> where T : EntityBase
    {
        #region Fields

        /// <summary>
        /// Источник данных.
        /// </summary>
        private readonly AppDbContext _dbContext;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="dbContext">Источник данных.</param>
        public Repository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        #endregion Constructors

        #region Utilities

        /// <summary>
        /// Строит запрос по спецификации.
        /// </summary>
        /// <param name="specification">Спецификация.</param>
        /// <returns>Запрос.</returns>
        private IQueryable<T> _BuildQueryOnSpecification(
           ISpecification<T> specification)
        {
            IQueryable<T> query = _dbContext.Set<T>().AsQueryable();

            if (specification.Includes.Count != 0)
            {
                query = specification.Includes.Aggregate(query,
                     (current, include) => current.Include(include));
            }

            if (specification.OrderBy != null)
            {
                query = query.OrderBy(specification.OrderBy);
            }

            if (specification.OrderByDescending != null)
            {
                query = query.OrderByDescending(
                    specification.OrderByDescending);
            }

            return query;
        }

        #endregion Utilities

        #region Methods 

        /// <summary>
        /// Асинхронное получение элемента.
        /// </summary>
        /// <param name="specification">Конкретная спецификация.</param>
        /// <returns>Элемент.</returns>
        public async Task<T> GetItemAsync(ISpecification<T> specification)
        {
            IQueryable<T> query = _BuildQueryOnSpecification(specification);

            return await query.Where(specification.Criteria).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Асинхронное получение коллекции элементов из базы.
        /// </summary>
        /// <param name="specification">Конкретная спецификация.</param>
        /// <returns>Коллекция элементов.</returns>
        public async Task<List<T>> GetListAsync(ISpecification<T> specification)
        {
            IQueryable<T> query = _BuildQueryOnSpecification(specification);

            return await query.Where(specification.Criteria).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// Асинхронное добавление элемента.
        /// </summary>
        /// <param name="entity">Добавляемая сущность.</param>
        /// <returns>Результат выполнения операции.</returns>
        public async Task AddAsync(T entity)
        {
            await _dbContext.Set<T>().AddAsync(entity);
        }

        /// <summary>
        /// Обновление элемента.
        /// </summary>
        /// <param name="entity">Обновляемая сущность.</param>
        public void Update(T entity)
        {
            _dbContext.Set<T>().Update(entity);
        }

        /// <summary>
        /// Обновление элемента.
        /// </summary>
        /// <param name="entity">Обновляемая сущность.</param>
        public void Remove(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
        }

        /// <summary>
        /// Асихронное сохранение.
        /// </summary>
        /// <returns>Успешное сохранение или нет.</returns>
        public async Task<bool> SaveChangesAsync()
        {
            return (await _dbContext.SaveChangesAsync() >= 0);
        }

        #endregion Methods
    }
}
