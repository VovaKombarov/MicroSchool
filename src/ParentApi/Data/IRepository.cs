using ParentApi.Data.Specifications;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ParentApi.Data
{
    public interface IRepository<T> where T : EntityBase
    {
        Task<T> GetItemAsync(ISpecification<T> specification);

        Task<List<T>> GetListAsync(ISpecification<T> specification);

        Task AddAsync(T entity);

        void Update(T entity);

        void Remove(T entity);

        Task<bool> SaveChangesAsync();

    }
}
