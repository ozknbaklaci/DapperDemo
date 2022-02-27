using System.Collections.Generic;
using System.Threading.Tasks;

namespace DapperDemo.Repository
{
    public interface IEntityRepository<T> where T : class, new()
    {
        Task<T> Find(int id);
        Task<List<T>> GetAll();
        Task<T> Add(T entity);
        Task<T> Update(T entity);
        Task Remove(int id);
    }
}
