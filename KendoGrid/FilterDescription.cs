using System.Collections.Generic;
using Newtonsoft.Json;

namespace KendoGrid
{

    public class FilterDescription
    {
        [JsonProperty("field")]
        public string Field { get; set; }
        [JsonProperty("operator")]
        public string Operator { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }

        [JsonProperty("logic")]
        public string Logic { get; set; }

        [JsonProperty("filters")]
        public List<FilterDescription> Filters { get; set; }
    }
    
}