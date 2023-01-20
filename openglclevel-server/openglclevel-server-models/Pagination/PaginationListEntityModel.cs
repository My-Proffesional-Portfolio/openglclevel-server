using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace openglclevel_server_models.Pagination
{

    public class PaginationListEntityModel<T>
    {
        public PaginationListEntityModel()
        {
            PagedList = new List<T>();
        }
        public List<T> PagedList { get; set; }
        public long PageNumber { get; set; }
        public long TotalCount { get; set; }
        public decimal TotalPages { get; set; }
    }
}
