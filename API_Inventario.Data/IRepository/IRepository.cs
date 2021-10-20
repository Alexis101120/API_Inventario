using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace API_Inventario.Data.IRepository
{
    public interface IRepository<T> where T : class
    {
        T Get(int id);

        Task<T> GetAsync(int id);

        IEnumerable<T> GetAll(
           Expression<Func<T, bool>> filter = null,
           Func<IQueryable<T>, IOrderedEnumerable<T>> orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null
         );

        Task<IEnumerable<T>> GetAllasync(
          Expression<Func<T, bool>> filter = null,
          Func<IQueryable<T>, IOrderedEnumerable<T>> orderBy = null,
           Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null
        );

        T GetFirstOrdefault(
             Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null
         );

        Task<T> GetFirstOrdefaultAsync(
            Expression<Func<T, bool>> filter = null,
           Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null
        );

        void Add(T entity);

        Task AddAsync(T entity);

        void Remove(int id);

        void Remove(T entity);
    }
}
