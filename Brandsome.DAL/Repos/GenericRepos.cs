using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using Brandsome.DAL.Models;
using Brandsome.DAL.Services;

namespace Brandsome.DAL.Repos
{
    public class GenericRepos<T> : IGenericRepos<T> where T : class
    {
        protected readonly BrandsomeDbContext _context;

        public GenericRepos(BrandsomeDbContext context)
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

        public async Task Delete(int id)
        {
            T t = await GetById(id);
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

        public bool CheckIfExists(Expression<Func<T, bool>> predicate)
        {
            return _context.Set<T>().Where(predicate).Any();
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

        public async Task<T> Update(T entity)
        {
            //_context.Set<T>().Update(entity);
            _context.Entry(entity).State = EntityState.Modified;
            //_context.Entry(entity).CurrentValues.SetValues(entity);

            await _context.SaveChangesAsync();
            return entity;
        }

        public IQueryable<T> GetAll(Expression<Func<T, bool>> predicate)
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

        public async Task <T> GetByIdWithPredicateAndIncludes(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes)
        {
            var query = GetAll();

            return await includes.Aggregate(query, (current, includeProperty) => current.Include(includeProperty)).SingleOrDefaultAsync(predicate);
        }

        public async Task<T> GetFirst(Expression<Func<T, bool>> predicate)
        {
            var query = await GetAll().Where(predicate).FirstOrDefaultAsync();
            return query;
            //return includes.Aggregate(query, (current, includeProperty) => current.Include(includeProperty)).SingleOrDefault(predicate);
        }
        public async Task<T> GetFirst(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes)
        {
            var query = await GetAll().Where(predicate).FirstOrDefaultAsync();
            return query;
            //return includes.Aggregate(query, (current, includeProperty) => current.Include(includeProperty)).SingleOrDefault(predicate);
        }


        public T GetByIdWithPredicateAndIncludesString(Expression<Func<T, bool>> predicate, string[] includes)
        {
            var query = GetAll();

            return includes.Aggregate(query, (current, includeProperty) => current.Include(includeProperty)).SingleOrDefault(predicate);
        }

        ////// EXECUTE SQL STORED PROCEDURE 


    }
}
