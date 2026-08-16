using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
    public class PageDataModel<T> where T : Base.BaseEntity
    {
        public PageDataModel(int pageindex , int pagecount , int totalcount , List<T> data) { 
            PageCount = pagecount;
            TotalCount = totalcount;
            PageIndex = pageindex;
            data = data;
        
            
        }
        public int PageIndex { get; set; }
        public int PageCount { get; set; }
        public int TotalCount { get; set; }
        public List<T> data { get; set; } = new List<T>();

    }
}
