using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace TeacherApi.Data.Specifications
{
    /// <summary>
    /// Базовая спецификация.
    /// </summary>
    /// <typeparam name="T">Обощенный тип, который используется в спецификации.</typeparam>
    public abstract class BaseSpecification<T> : ISpecification<T>
    {
        #region Properties 

        /// <summary>
        /// Выражение содержащие критерий спецификации.
        /// </summary>
        public Expression<Func<T, bool>> Criteria { get; protected set; }

        /// <summary>
        /// Коллекция выражений содержащая коллекцию связанных данных.
        /// </summary>
        public List<Expression<Func<T, object>>> Includes { get; } = new List<Expression<Func<T, object>>>();

        /// <summary>
        /// Добавляет выражение, которое упорядочивает по возрастанию.
        /// </summary>
        public Expression<Func<T, object>> OrderBy { get; private set; }

        /// <summary>
        /// Добавляет выражение, которое упорядочивает по убыванию.
        /// </summary>
        public Expression<Func<T, object>> OrderByDescending { get; private set; }

        #endregion Properties

        #region Constructors

        /// <summary>
        /// Конструктор.
        /// </summary>
        public BaseSpecification()
        {

        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="criteria">Выражение содержащее критерий.</param>
        public BaseSpecification(Expression<Func<T, bool>> criteria)
        {
            Criteria = criteria;
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Добавляет связанные данные.
        /// </summary>
        /// <param name="includeExpression">Выражение добавляющее связанные данные.</param>
        protected void AddInclude(Expression<Func<T, object>> includeExpression)
        {
            Includes.Add(includeExpression);
        }

        /// <summary>
        /// Добавляет выражение, которое упорядочивает по возрастанию.
        /// </summary>
        /// <param name="orderByExpression">Выражение добавляющее упорядочивание по возрастанию.</param>
        protected void AddOrderBy(Expression<Func<T, object>> orderByExpression)
        {
            OrderBy = orderByExpression;
        }

        /// <summary>
        /// Добавляет выражение, которое упорядочивает по убыванию.
        /// </summary>
        /// <param name="orderByDescExpression">Выражение добавляющее упорядочивание по убыванию.</param>
        protected void AddOrderByDescending(Expression<Func<T, object>> orderByDescExpression)
        {
            OrderByDescending = orderByDescExpression;
        }

        #endregion Methods

    }
}
