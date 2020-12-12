using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EngineeringUnitsCore.DLA.AccessorContracts
{
    public interface IAccessorBase<T>
    {
        Task<IList<T>> GetAll();

        Task<T> Get(string id);

        Task<List<T>> GetByCondition(Expression<Func<T, bool>> expression);

        void Create(T entity);

        void Update(T entity);

        void Delete(T entity);
    }
}