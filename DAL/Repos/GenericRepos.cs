using DAL.Data;
using DAL.Services;
//using DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DAL.Repos
{
    public class GenericRepos<T> : IGenericRepos<T> where T : class
    {
        protected readonly FintechLeapDbContext _context;

        public GenericRepos(FintechLeapDbContext context)
        {
            _context = context;
        }

        public async Task<T> Create(T entity)
        {
            try
            {
                await _context.Set<T>().AddAsync(entity);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
            return entity;
        }


        public async Task<T> Add(T entity) // without Save
        {
            try
            {
                await _context.Set<T>().AddAsync(entity);
            }
            catch (Exception)
            {
                throw;
            }
            return entity;
        }

        public async Task AddRange(List<T> entities) // without Save
        {
            try
            {
                await _context.Set<T>().AddRangeAsync(entities);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task Delete(int id)
        {
            T t = await GetById(id);
            if (t != null)
            {
                _context.Entry(t).State = EntityState.Deleted;
                await _context.SaveChangesAsync();

            }
        }

        public async Task DeleteRange(List<T> entities)
        {
            if (entities.Count() > 0)
            {
                foreach (var item in entities)
                {
                    _context.Entry(item).State = EntityState.Deleted;
                    await _context.SaveChangesAsync();
                }

            }
        }

        public async Task DeleteByPredicate(Expression<Func<T, bool>> predicate)
        {
            T t = await GetFirst(predicate);
            if (t != null)
            {
                _context.Entry(t).State = EntityState.Deleted;
                await _context.SaveChangesAsync();

            }
        }

        public IQueryable<T> GetAll()
        {
            return _context.Set<T>().AsNoTracking();
        }

        public IQueryable<T> GetAllWithTracking()
        {
            return _context.Set<T>().AsTracking();
        }

        public IQueryable<T> GetAll(Expression<Func<T, bool>> predicate)
        {
            return GetAll().Where(predicate);
        }

        public IQueryable<T> GetAllWithTracking(Expression<Func<T, bool>> predicate)
        {
            return _context.Set<T>().Where(predicate);
        }


        public IQueryable<T> GetAllWithInclude(params Expression<Func<T, object>>[] includes)
        {
            var query = includes.Aggregate(GetAll(), (current, includeProperty) => current.Include(includeProperty)).AsNoTracking();
            return query;
        }

        public async Task<T> GetById(int Id)
        {
            return await _context.Set<T>().FindAsync(Id);
        }

        public async Task<T> GetById(string id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task<T> GetFirst(Expression<Func<T, bool>> predicate)
        {
            var query = await GetAll().Where(predicate).FirstOrDefaultAsync();
            return query;
            //return includes.Aggregate(query, (current, includeProperty) => current.Include(includeProperty)).SingleOrDefault(predicate);
        }


        public async Task<bool> CheckIfExists(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().Where(predicate).AnyAsync();
        }

        public async Task<T> Update(T entity)
        {
            //_context.Set<T>().Update(entity);
            //_context.Entry(entity).State = EntityState.Modified; // before core 
            _context.Attach(entity).State = EntityState.Modified; // After core 
            //_context.Entry(entity).CurrentValues.SetValues(entity);

            await _context.SaveChangesAsync();
            return entity;
        }

        public IQueryable<T> GetAllWithPredicate(Expression<Func<T, bool>> predicate)
        {
            return GetAll().Where(predicate);
        }

        public IQueryable<T> GetAllWithPredicateAndIncludes(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes)
        {
            var query = includes.Aggregate(GetAll(), (current, includeProperty) => current.Include(includeProperty)).AsNoTracking();

            return query.Where(predicate);
        }

        public IQueryable<T> GetAllWithPredicateAndIncludesString(Expression<Func<T, bool>> predicate, string[] includes)
        {
            //var query = GetAll().Where(predicate);
            var query = includes.Aggregate(GetAll(), (current, includeProperty) => current.Include(includeProperty)).AsNoTracking();
            return query.Where(predicate);
        }

        public T GetByIdWithPredicateAndIncludes(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes)
        {
            var query = GetAll();

            return includes.Aggregate(query, (current, includeProperty) => current.Include(includeProperty)).SingleOrDefault(predicate); // Aggregate: yaane faw2 baadun ysiro
        }

        public T GetByIdWithPredicate(Expression<Func<T, bool>> predicate)
        {
            var query = GetAll().Where(predicate).FirstOrDefault();
            return query;
            //return includes.Aggregate(query, (current, includeProperty) => current.Include(includeProperty)).SingleOrDefault(predicate);
        }


        public T GetByIdWithPredicateAndIncludesString(Expression<Func<T, bool>> predicate, string[] includes)
        {
            var query = GetAll();

            return includes.Aggregate(query, (current, includeProperty) => current.Include(includeProperty)).SingleOrDefault(predicate);
        }

    }
}
