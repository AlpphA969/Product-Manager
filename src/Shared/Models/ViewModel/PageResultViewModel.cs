using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModel
{
    public class PageResultViewModel<T> : object
    {
    
        public int PageIndex { get; set; }
        public int PageCount { get; set; }
        public int TotalCount { get; set; }
        public List<T> data { get; set; } = new List<T>();
    }
}
