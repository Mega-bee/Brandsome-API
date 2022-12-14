using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Brandsome.DAL.Services
{
    public interface IGenericRepos<T>
    {
        IQueryable<T> GetAll();

        Task<T> GetById(int Id);

        Task<T> GetById(string id);

        Task<T> Create(T entity);

        Task<T> Update(T entity);

        Task Delete(int id);
        bool CheckIfExists(Expression<Func<T, bool>> predicate);
        Task<T> GetFirst(Expression<Func<T, bool>> predicate);
        Task<T> GetByIdWithPredicateAndIncludes(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);
        IQueryable<T> GetAll(Expression<Func<T, bool>> predicate);
        IQueryable<T> GetAllWithPredicateAndIncludes(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);
        T GetByIdWithPredicateAndIncludesString(Expression<Func<T, bool>> predicate, string[] includes);
        IQueryable<T> GetAllWithPredicateAndIncludesString(Expression<Func<T, bool>> predicate, string[] includes);
        IQueryable<T> GetAllWithInclude(params Expression<Func<T, object>>[] includes);
        //void ExecuteRawSQL(string query, params object[] parameters);
    }
}
