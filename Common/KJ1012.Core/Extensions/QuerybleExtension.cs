using System.Linq;
using KJ1012.Core.Data;

namespace KJ1012.Core.Extensions
{
    public static class QuerybleExtension
    {
        public static  (int Count,IQueryable<T> Query) Pager<T>(this IQueryable<T> query, Pager pager)
        {
            return (query.Count(),query.Skip((pager.CurrentPage - 1) * pager.PageSize)
                .Take(pager.PageSize));
        }
        public static (int Count, IQueryable<dynamic> Query) Pager(this IQueryable<dynamic> query, Pager pager)
        {
            return (query.Count(), query.Skip((pager.CurrentPage - 1) * pager.PageSize)
                .Take(pager.PageSize));
        }
        public static (int Count, IQueryable<dynamic> Query) Pager(this IQueryable<dynamic> query, Pager pager,int defaultCount)
        {
            return (defaultCount, query.Skip((pager.CurrentPage - 1) * pager.PageSize)
                .Take(pager.PageSize));
        }
    }
}
