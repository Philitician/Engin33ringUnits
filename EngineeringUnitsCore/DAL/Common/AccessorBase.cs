using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using EngineeringUnitsCore.DAL.AccessorContracts;
using EngineeringUnitsCore.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Caching.Memory;

namespace EngineeringUnitsCore.DAL.Common
{
    public abstract class AccessorBase<T> : DataAccess, IAccessorBase<T> where T : class
    {
        protected AccessorBase(EngineeringUnitsContext context) : base(context) { }

        public async Task<IList<T>> GetAll()
        {
            var key = typeof(T).Name;
            if (Cache.TryGetValue<IList<T>>(key, out var cachedEntry)) 
                return cachedEntry;

            Console.WriteLine($"{key} not cached");
            cachedEntry = await Context.Set<T>().ToListAsync();
            
            var entryOptions = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(1));
            
            // adds unit to cache
            Cache.Set(key, cachedEntry, entryOptions);

            return cachedEntry;
        }

        public async Task<T> Get(string id)
        {
            return await Context.Set<T>().FindAsync(id);
            
        }
        
        public async Task<IQueryable<T>> Get(string id, Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null)
        {
            var key = typeof(T).Name + "_" + id;
            if (Cache.TryGetValue<IQueryable<T>>(key, out var entity)) return entity;
            
            entity = Context.Set<T>().Where(predicate);
            
            if (include != null)
                entity = include(entity);
            
            var entryOptions = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(1));
            
            // adds unit to cache
            Cache.Set(id, entity, entryOptions);

            return entity;
        }
        
        public async Task<IQueryable<T>> Get(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null)
        {
            var entity = Context.Set<T>().Where(predicate);
            
            if (include != null)
                entity = include(entity);
            
            return entity;
        }

        public async Task<List<T>> GetByCondition(Expression<Func<T, bool>> expression)
        {
            return await Context.Set<T>().Where(expression).ToListAsync();
        }

        public void Create(T entity)
        {
            Context.Set<T>().Add(entity);
        }

        public void Update(T entity)
        {
            Context.Set<T>().Update(entity);
        }

        public void Delete(T entity)
        {
            Context.Set<T>().Remove(entity);
        }
    }
}