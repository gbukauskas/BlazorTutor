using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WEBtransitions.ClassLibraryDatabase.CustomPager
{
    public class PgPostData: ICloneable
    {
        /// <summary>
        /// 10 is default value. <see cref="StateData.defaultPageSize"/>
        /// </summary>
        public int MaxButtons { get; set; } //+
        /// <summary>
        /// Constructor sets value to 0. Real value will occur after reading dbo.Customers table
        /// </summary>
        public int RowCount { get; set; }   //+
        /// <summary>
        /// Constructor sets value to 0. Real value will occur after reading dbo.Customers table
        /// </summary>
        public int PageCount { get; set; }  //+
        /// <summary>
        /// Constructor sets value to 1. The User will set real value
        /// </summary>
        public int PageNumber { get; set; }     //+
        public string PageNumberStr {
            get
            {
                return this.PageNumber.ToString();
            }
            set
            {
                int tmpValue = 0;
                this.PageNumber = string.IsNullOrEmpty(value) || !int.TryParse(value, out tmpValue)
                    ? 1
                    : tmpValue;
            }
        }
        /// <summary>
        /// 9 is default value
        /// </summary>
        public int PageSize { get; set; } = 0;  //+
        /// <summary>
        /// Every component writes here the name.
        /// </summary>
        public string? BaseUrl { get; set; }    //+

        public PgPostData(int maxButtons = 0, int rowCount = 0, int pageCount = 0, int pageNumber = 0, int pageSize = 0, string? baseUrl = "")
        {
            MaxButtons = maxButtons;
            RowCount = rowCount;
            PageCount = pageCount;
            PageNumber = pageNumber;
            PageSize = pageSize;
            BaseUrl = baseUrl;
        }

        public object Clone()
        {
            return new PgPostData(this.MaxButtons, this.RowCount, this.PageCount, this.PageNumber, this.PageSize, this.BaseUrl);
        }
    }
}
