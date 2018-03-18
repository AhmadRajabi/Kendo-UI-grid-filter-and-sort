using System.Collections.Generic;

namespace KendoGrid
{
    public class KendoGridResult<T>
    {
        public List<T> Data { get; set; }
        public int Total { get; set; }
    }
}