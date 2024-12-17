using Microsoft.CodeAnalysis;
using System.Linq.Expressions;

namespace JobExpressBack.Models.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        //CRUD commun entre tous les models
        Task<IEnumerable<T>> GetAll();
        Task<T> GetById(int id);
        Task Add(T entity);
        Task Update(T entity);
        Task Delete(int id);

      

    }
}
