using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ClassLibraryDatabase.CustomPager
{
    public record class PgPostData(int MaxButtons = 10)
    {
        public int RowCount { get; set; }   //+
        /// <summary>
        /// Constructor sets value to 0. Real value will occur after reading dbo.Customers table
        /// </summary>
        public int PageCount { get; set; }  //+

        /// <summary>
        /// Constructor sets value to 1. The User will set real value
        /// </summary>
        public int PageNumber { get; set; }     //+

        [Required]
        [RegularExpression(@"^\d+$", ErrorMessage = "PageNumberStr number contains invalid characters.")]
        public string PageNumberStr
        {
            get
            {
                return this.PageNumber .ToString();
            }
            set
            {
                int tmpvalue = 0;
                this.PageNumber = string.IsNullOrEmpty(value) || !int.TryParse(value, out tmpvalue)
                    ? 1
                    : tmpvalue;
            }
        }
        /// <summary>
        /// 9 is default value
        /// </summary>
        public int PageSize { get; set; }  //+

        /// <summary>
        /// Every component writes here the name.
        /// </summary>
        public string? BaseUrl { get; set; }    //+
    }
}
