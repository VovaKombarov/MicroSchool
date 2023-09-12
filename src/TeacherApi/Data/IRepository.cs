using System.Collections.Generic;
using System.Threading.Tasks;
using TeacherApi.Data.Specifications;

namespace TeacherApi.Data
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
