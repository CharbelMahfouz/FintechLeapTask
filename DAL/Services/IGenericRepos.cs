using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DAL.Services
{
    public interface IGenericRepos<T>
    {
        IQueryable<T> GetAll();
        IQueryable<T> GetAll(Expression<Func<T, bool>> predicate);
        IQueryable<T> GetAllWithTracking();
        IQueryable<T> GetAllWithTracking(Expression<Func<T, bool>> predicate);
        Task<T> Add(T entity);
        Task AddRange(List<T> entities);
        Task<T> GetById(int Id);
        //bool CheckIfExists(Expression<Func<T, bool>> predicate);
        Task<bool> CheckIfExists(Expression<Func<T, bool>> predicate);
        Task<T> GetById(string id);

        Task<T> GetFirst(Expression<Func<T, bool>> predicate);

        Task<T> Create(T entity);

        Task<T> Update(T entity);

        Task Delete(int id);
        Task DeleteRange(List<T> entities);
        Task DeleteByPredicate(Expression<Func<T, bool>> predicate);
        T GetByIdWithPredicate(Expression<Func<T, bool>> predicate);
        T GetByIdWithPredicateAndIncludes(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);
        IQueryable<T> GetAllWithPredicate(Expression<Func<T, bool>> predicate);
        IQueryable<T> GetAllWithPredicateAndIncludes(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);
        T GetByIdWithPredicateAndIncludesString(Expression<Func<T, bool>> predicate, string[] includes);
        IQueryable<T> GetAllWithPredicateAndIncludesString(Expression<Func<T, bool>> predicate, string[] includes);
        IQueryable<T> GetAllWithInclude(params Expression<Func<T, object>>[] includes);
    }
}
