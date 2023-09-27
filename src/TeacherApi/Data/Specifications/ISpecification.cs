using System.Collections.Generic;
using System.Linq.Expressions;
using System;

namespace TeacherApi.Data.Specifications
{
    /// <summary>
    /// Интерфейс спецификаций.
    /// </summary>
    /// <typeparam name="T">Обобщенный тип использующий спецификацию.</typeparam>
    public interface ISpecification<T>
    {
        /// <summary>
        /// Выражение содержащие критерий спецификации.
        /// </summary>
        Expression<Func<T, bool>> Criteria { get; }

        /// <summary>
        /// Коллекция выражений содержащая коллекцию связанных данных.
        /// </summary>
        List<Expression<Func<T, object>>> Includes { get; }

        /// <summary>
        /// Добавляет выражение, которое упорядочивает по возрастанию.
        /// </summary>
        Expression<Func<T, object>> OrderBy { get; }

        /// <summary>
        /// Добавляет выражение, которое упорядочивает по убыванию.
        /// </summary>
        Expression<Func<T, object>> OrderByDescending { get; }
    }
}
