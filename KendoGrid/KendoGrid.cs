using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace KendoGrid
{
    public static class KendoGrid
    {
        public static async Task<KendoGridResult<T>> ToDataSourceAsync<T>(this IQueryable<T> query, Request request)
        {
            var filter = Filter<T>.GetExpression(request.Filter);

            if (filter != null)
            {
                query = query.Where(filter);
            }

            var sort = Sort.GetSortExpression(request.sort);

            if (sort != null)
            {
                query = query.OrderBy(sort);
            }

            var total =await  query.CountAsync();

            if (request.skip + request.take > 0)
            {
                query = query.Skip(request.skip).Take(request.take);
            }

            var result = new KendoGridResult<T>
            {
                Data =await  query.ToListAsync(),
                Total = total
            };

            return result;
        }


    }
}