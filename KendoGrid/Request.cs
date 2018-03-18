using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace KendoGrid
{
    public class Request
    {
        public int skip { get; set; }
        public int take { get; set; }
        //public int page { get; set; }
        //public int pageSize { get; set; }
        public List<SortDescription> sort { get; set; }
        [JsonProperty("filter")]
        public FilterDescription Filter { get; set; }

        public bool HasFilter()
        {
            return Filter?.Filters != null && Filter.Filters.Any();
        }
    }
}