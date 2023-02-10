using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opteamix.AuthorizationFramework.Database.DTO
{
    public class PageResult<T> where T : class
    {
        public int CurrentPage { get; set; }
        public int PageCount { get; set; }
        public int RowCount { get; set; }
        public int PageSize { get; set; }
        public IEnumerable<T> Result { get; set; }
    }
}
