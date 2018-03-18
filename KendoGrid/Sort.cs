using System.Collections.Generic;
using System.Linq;

namespace KendoGrid
{
    internal class Sort
    {
        public static string GetSortExpression(List<SortDescription> sortDescriptions)
        {
            if (sortDescriptions == null || !sortDescriptions.Any())
                return null;

            var list = sortDescriptions.Select(sortDescription => string.Format("{0} {1}", sortDescription.field, sortDescription.dir)).ToList();
            var result = string.Join(" , ", list);
            return result;
        }

    }
}