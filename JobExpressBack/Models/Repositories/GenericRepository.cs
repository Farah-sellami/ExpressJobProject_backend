using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Reflection;

namespace JobExpressBack.Models.Repositories
{
    /*Implémentez l'interface générique IGenericRepository dans une classe qui 
     * contiendra les implémentations par défaut des méthodes CRUD.*/
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly ExJobDBContext exJobDBContext;
        private readonly DbSet<T> dbSet;

        public GenericRepository(ExJobDBContext exJobDBContext)
        {
            this.exJobDBContext = exJobDBContext;
            this.dbSet=exJobDBContext.Set<T>();
        }

        public async Task Add(T entity)
        {
            await dbSet.AddAsync(entity);
            await exJobDBContext.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var entity = await dbSet.FindAsync(id);
            if (entity != null)
            {
                dbSet.Remove(entity);
                await exJobDBContext.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await dbSet.ToListAsync();
        }

        public async Task<T> GetById(int id)
        {
            return await dbSet.FindAsync(id);
        }

        public async Task Update(T entity)
        {
            dbSet.Update(entity);
            await exJobDBContext.SaveChangesAsync();
        }   

    }
}
