using Microsoft.EntityFrameworkCore;

namespace PyroDB.Helpers
{
    public static class QueryHelpers
    {
        public static IQueryable<T> CustomQuery<T>(this IQueryable<T> query, string? filter, string? order, int? pageNo, int? pageSize) where T : class
        {
            if (filter != null)
                throw new NotImplementedException("Filters not yet supported");

            if (order != null)
                query = query.Order(order);

            if (pageNo != null && pageSize != null)
                query = query.Paging(pageNo.Value, pageSize.Value);

            return query;
        }


        public static IQueryable<T> Order<T>(this IQueryable<T> query, string order) where T : class
        {
            bool desending = order.EndsWith("_desc");
            if (desending)
            {
                var propertyName = order.Substring(0, order.Length - 5);
                return query.OrderByDescending(e => EF.Property<object>(e, propertyName));
            }
            else
            {
                var propertyName = order;
                return query.OrderBy(e => EF.Property<object>(e, propertyName));
            }
        }


        public static IQueryable<T> Paging<T>(this IQueryable<T> query, int pageNo, int pageSize) where T : class
        {
            return query.Skip(pageNo * pageSize).Take(pageSize);
        }



    }
}
