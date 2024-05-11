using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Pagination
{
    public static class QueryableExtensions
    {
        const int DefaultPage = 1;
        const int DefaultPageSize = 10;

        public static IQueryable<T> Paginate<T>(this IQueryable<T> query, int? page = DefaultPage, int? pageSize = DefaultPageSize)
        {
            if (query is null)
            {
                throw new ArgumentNullException(nameof(query));
            }


            return query.Skip((page.Value - 1) * pageSize.Value).Take(pageSize.Value);
        }

         public static IQueryable<T> Paginate<T>(
         this IQueryable<T> query,
         int page = DefaultPage,
         int pageSize = DefaultPageSize,
         Expression<Func<T, bool>> filter = null,
         Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
         Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null)
        {
            if (query == null)
                throw new ArgumentNullException(nameof(query));

            if (page <= 0 || pageSize <= 0)
                throw new ArgumentException("Page and page size must be positive integers.");

            if (filter != null)
                query = query.Where(filter);

            if (include != null)
                query = include(query);

            if (orderBy != null)
                query = orderBy(query);
              

            return query.Skip((page - 1) * pageSize).Take(pageSize);
        }
    
        public static long CountBack<T>(
         this IQueryable<T> query,
         int page = DefaultPage,
         int pageSize = DefaultPageSize,
         Expression<Func<T, bool>> filter = null,
         Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
         Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null)
        {
            if (query is null)
                throw new ArgumentNullException(nameof(query));

            if (page <= 0 || pageSize <= 0)
                throw new ArgumentException("Page and page size must be positive integers.");

            if (filter != null)
                query = query.Where(filter);

            if (include != null)
                query = include(query);

            if (orderBy != null)
                query = orderBy(query);

            return query.Count<T>();
        }
        public static IQueryable<T> FirstPage<T>(this IQueryable<T> query, int pageSize)
            => query.Take(pageSize);

        public static IQueryable<T> FirstPage<T>(this IQueryable<T> query, int pageSize,
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null)
        {
            if (query is null)
                throw new ArgumentNullException(nameof(query));

            if ( pageSize <= 0)
                throw new ArgumentException("page size must be positive integers.");

            if (filter != null)
                query = query.Where(filter);

            if (include != null)
                query = include(query);

            if (orderBy != null)
                query = orderBy(query);
            
         
          return  query.Take(pageSize);
        }

        public static IQueryable<T> LastPage<T>(this IQueryable<T> query, int pageSize)
            => query.TakeLast(pageSize);

        public static IQueryable<T> LastPage<T>(this IQueryable<T> query, int pageSize ,
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null)
        {
            if (query is null)
                throw new ArgumentNullException(nameof(query));

            if (pageSize <= 0)
                throw new ArgumentException("page size must be positive integers.");

            if (filter != null)
                query = query.Where(filter);

            if (include != null)
                query = include(query);

            if (orderBy != null)
                query = orderBy(query);
            
         
            return query.TakeLast(pageSize);
        }
        public static int CountOfPages<T>(this IQueryable<T> query, int pageSize)
        {
            var total = query.Count();
            return (total / pageSize) + ((total % pageSize) > 0 ? 1 : 0);
        }

        public static IQueryable<T> WhereIf<T>(this IQueryable<T> query, bool condition, Expression<Func<T, bool>> predicate)
        {
            return condition
                ? query.Where(predicate)
                : query;
        }

    }
}