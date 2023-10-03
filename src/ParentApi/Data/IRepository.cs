using ParentApi.Data.Specifications;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ParentApi.Data
{
    /// <summary>
    /// Интерфейс для работы с репозитоями.
    /// </summary>
    /// <typeparam name="T">Обощенный тип сущности БД.</typeparam>
    public interface IRepository<T> where T : EntityBase
    {
        /// <summary>
        /// Асинхронное получение элемента.
        /// </summary>
        /// <param name="specification">Конкретная спецификация.</param>
        /// <returns>Элемент.</returns>
        Task<T> GetItemAsync(ISpecification<T> specification);

        /// <summary>
        /// Асинхронное получение коллекции элементов из базы.
        /// </summary>
        /// <param name="specification">Конкретная спецификация.</param>
        /// <returns>Коллекция элементов.</returns>
        Task<List<T>> GetListAsync(ISpecification<T> specification);

        /// <summary>
        /// Асинхронное добавление элемента.
        /// </summary>
        /// <param name="entity">Добавляемая сущность.</param>
        /// <returns>Результат выполнения операции.</returns>
        Task AddAsync(T entity);

        /// <summary>
        /// Обновление элемента.
        /// </summary>
        /// <param name="entity">Обновляемая сущность.</param>
        void Update(T entity);

        /// <summary>
        /// Удаление элемента.
        /// </summary>
        /// <param name="entity">Удаляемая сущность.</param>
        void Remove(T entity);

        /// <summary>
        /// Асихронное сохранение.
        /// </summary>
        /// <returns>Успешное сохранение или нет.</returns>
        Task<bool> SaveChangesAsync();
    }
}
