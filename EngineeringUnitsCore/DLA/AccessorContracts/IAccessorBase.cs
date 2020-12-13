using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Query;

namespace EngineeringUnitsCore.DLA.AccessorContracts
{
    public interface IAccessorBase<T>
    {
        Task<IList<T>> GetAll();

        Task<T> Get(string id);

        public Task<IQueryable<T>> Get(
            string id, 
            Expression<Func<T, bool>> predicate,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null);

        Task<List<T>> GetByCondition(Expression<Func<T, bool>> expression);

        void Create(T entity);

        void Update(T entity);

        void Delete(T entity);
    }
}