using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WEBtransitions.ClassLibraryDatabase.CustomPager
{
    public class PgResponse<T> where T : class
    {
        /// <summary>
        /// Number of records in the dataset
        /// </summary>
        public int TotalRecords { get; set; }

        /// <summary>
        /// Total number of pages
        /// </summary>
        public int TotalPages { get; set; } = 0;

        /// <summary>
        /// Number of records in the page
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Number of current page. Page numbering starts from 1.
        /// </summary>
        public int PageNumber { get; set; }

        /// <summary>
        /// Content of the current page.
        /// </summary>
        public IEnumerable<T> Items { get; set; }

        public PgResponse() 
        {
            TotalRecords = 0;
            TotalPages = 0; 
            PageSize = 0;
            PageNumber = 0;
            Items = Enumerable.Empty<T>();
        }

        /// <summary>
        /// The function constucts object PgResponse
        /// </summary>
        /// <param name="recordCount">Total count of records</param>
        /// <param name="pageSize">Size of the page</param>
        /// <param name="pageNumber">Page number; numbering starts from 1</param>
        /// <param name="totalPages">Total pages</param>
        /// <param name="items">Items in the current page</param>
        /// <returns></returns>
        public PgResponse(int recordCount, int pageSize, int pageNumber, int totalPages, IEnumerable<T> items)
        {
            this.TotalRecords = recordCount;
            this.TotalPages = totalPages;
            this.PageSize = pageSize;
            this.PageNumber = pageNumber;
            this.Items = items;
        }
    }
}
