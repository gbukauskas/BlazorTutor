using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WEBtransitions.ClassLibraryDatabase.CustomPager
{
    public class PgPostData: ICloneable
    {
        public string? Id { get; set; }
        public int MaxButtons { get; set; }
        public int PageCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string? BaseUrl { get; set; }

        public PgPostData(string? id, int maxButtons = 0, int pageCount = 0, int pageNumber = 0, int pageSize = 0, string? baseUrl = "")
        {
            Id = id;
            MaxButtons = maxButtons;
            PageCount = pageCount;
            PageNumber = pageNumber;
            PageSize = pageSize;
            BaseUrl = baseUrl;
        }

        public object Clone()
        {
            return new PgPostData(this.Id, this.MaxButtons, this.PageCount, this.PageNumber, this.PageSize, this.BaseUrl);
        }
    }
}
