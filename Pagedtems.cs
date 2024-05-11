using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Pagination
{
    public class Pagedtems<T>
    {
        protected long PageIndex { get; }

        public long PageSize { get; }

        public long PagesCount { get; }

        public long ItemsCount { get; }

        public IReadOnlyCollection<T> Items { get; }
        public Pagedtems(IQueryable<T> query, int pageIndex, int pageSize, long? totalItemsCount)
        {
            ArgumentNullException.ThrowIfNull(query);
            long itemsCount = totalItemsCount is null ? query.CountBack(pageIndex,pageSize) : totalItemsCount.GetValueOrDefault();
            
            Items = query.Paginate(pageIndex, pageSize).ToImmutableArray();
            PageIndex = pageIndex;
            PageSize = pageSize;
            ItemsCount = itemsCount;
            PagesCount = pageSize != 0L ? (long)Math.Ceiling(ItemsCount / (Decimal)pageSize) : 0L;
        }
        public Pagedtems(IQueryable<T> query, int pageIndex, int pageSize, long? totalItemsCount, Expression<Func<T, bool>> filter = null,
         Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null,
         Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null)
        {
            ArgumentNullException.ThrowIfNull(query);
            long itemsCount = totalItemsCount is null ? query.CountBack(pageIndex, pageSize) : totalItemsCount.GetValueOrDefault();

            Items = query.Paginate(pageIndex, pageSize, filter, include, orderBy).ToImmutableArray();
            PageIndex = pageIndex;
            PageSize = pageSize;
            ItemsCount = itemsCount;
            PagesCount = pageSize != 0L ? (long)Math.Ceiling(ItemsCount / (Decimal)pageSize) : 0L;
        }
        public async Task<long> CountItemsAsync(IQueryable<T> query )
        {
            ArgumentNullException.ThrowIfNull(query);

            int result = await query.CountAsync();

            return result;
        }
    }
}
