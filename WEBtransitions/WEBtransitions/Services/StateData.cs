using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.WebUtilities;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using WEBtransitions.ClassLibraryDatabase.CustomPager;
using WEBtransitions.ClassLibraryDatabase.DBContext;
using WEBtransitions.Services.Interfaces;

namespace WEBtransitions.Services
{
    public class StateForComponent: ICloneable
    {
        public string AppName { get; set; }             //+
        public string UserId { get; set; }              //+
        public string ComponentName { get; set; }       //+
        public string? SortState {  get; set; } = string.Empty; //+
        public PgPostData? PagerState { get; set; } = null;

        /// <summary>
        /// name of the field, Value, true for date values
        /// </summary>
        public Tuple<string, string, bool> FilterState { get; set; }
        public string LastInsertedId { get; set; }


        public StateForComponent(string appName, string userId, string componentName, int buttonCount = 10, int pageSize = 9) 
        {
            this.AppName = appName;
            this.UserId = userId;
            this.ComponentName = componentName;
            this.SortState = string.Empty;
            this.PagerState = new PgPostData(buttonCount, 0, 0, 1, pageSize, "Customers"); // rowCount, pageCount, pageNumber will get real values after reading the database
            this.FilterState = new Tuple<string, string, bool>("", "", false);
            this.LastInsertedId = "";
        }

        public object Clone()
        {
            Debug.Assert(this.ComponentName != null);
            var rzlt = new StateForComponent(this.AppName, this.UserId, this.ComponentName)
            {
                SortState = this.SortState,
                FilterState = this.FilterState,
                LastInsertedId = this.LastInsertedId
            };
            if (this.PagerState != null)
            {
                rzlt.PagerState = (PgPostData)this.PagerState.Clone();
            }

            return rzlt;
        }

        /// <summary>
        /// Set values in the <code>this.currentState.PagerState</code>
        /// </summary>
        /// <param name="argument">Page size or Sort definition</param>
        /// <param name="navManager"><see cref="NavigationManager"/></param>
        /// <param name="baseUrl">Base URL</param>
        public void PreparePaging(string argument, NavigationManager navManager, string baseUrl, int pageNumber)
        {
            Debug.Assert(this.PagerState != null);
            int pageSize;

            if (int.TryParse(argument, out pageSize)) // this.PagerState.PageSize has default value if parsing fails.
            {
                this.PagerState.PageSize = pageSize;
            }
            this.PagerState.PageNumber = pageNumber;

            var uri = navManager.ToAbsoluteUri(navManager.Uri);
            if (QueryHelpers.ParseQuery(uri.Query).TryGetValue("current-page", out var _initialCount))
            {
                this.PagerState.PageNumber = Convert.ToInt32(_initialCount);
            }
            this.PagerState.BaseUrl = $"{baseUrl}/page";  //"Customers/page", "Employees/page";
        }

// Remove reference from customers
        /// <summary>
        /// Set values in the <code>this.currentState.FilterState</code>
        /// </summary>
        public void PrepareFiltering(NavigationManager navManager)
        {
            string fieldName = string.Empty;
            string searchValue = string.Empty;

            var uri = navManager.ToAbsoluteUri(navManager.Uri);
            if (QueryHelpers.ParseQuery(uri.Query).TryGetValue("field-name", out var _fieldName))
            {
                fieldName = Convert.ToString(_fieldName);
            }
            if (QueryHelpers.ParseQuery(uri.Query).TryGetValue("search-value", out var _searchValue))
            {
                searchValue = Convert.ToString(_searchValue);
            }
            this.FilterState = Tuple.Create(fieldName, searchValue, false);
        }


        public static explicit operator StateForComponent(AppState dbState)
        {
            return new StateForComponent(dbState.AppName, dbState.UserId, dbState.ComponentName,
                                         dbState.PagerButtonCount ?? 5,
                                         dbState.PagerPageSize ?? 15)
            {
                UserId = dbState.UserId,
                SortState = dbState.SortState,
                PagerState = new PgPostData(dbState.PagerButtonCount ?? 5,
                                            dbState.PagerRowCount ?? 0, dbState.PagerPageCount ?? 0,
                                            dbState.PagerPageNumber ?? 1, dbState.PagerPageSize ?? 15,
                                            dbState.PagerBaseUrl),
                FilterState = new Tuple<string, string, bool>(dbState.FilterFieldName ?? "", dbState.FilterFieldValue ?? "", dbState.FilterIsDateValue == 1),
                LastInsertedId = ""
            };
        }
        public static explicit operator AppState(StateForComponent currentState)
        {
            Debug.Assert(currentState != null);
            string baseUrl = $"{currentState.ComponentName}/page";
            var rzlt = new AppState()
            {
                AppName = "WEBtransitions",
                UserId = currentState.UserId,
                ComponentName = currentState.ComponentName,
                SortState = currentState.SortState,
                FilterFieldName = currentState.FilterState.Item1,
                FilterFieldValue = currentState.FilterState.Item2,
                FilterIsDateValue = currentState.FilterState.Item3 ? (short)1 : (short)0,
                PagerBaseUrl = baseUrl,
                IsDeleted = 0
            };
            if (currentState.PagerState == null)
            {
                rzlt.PagerButtonCount = null;
                rzlt.PagerRowCount = null;
                rzlt.PagerPageCount = null;
                rzlt.PagerPageNumber = null;
                rzlt.PagerPageSize = null;
            } 
            else
            {
                rzlt.PagerButtonCount = currentState.PagerState.MaxButtons;
                rzlt.PagerRowCount = currentState.PagerState.RowCount;
                rzlt.PagerPageCount = currentState.PagerState.PageCount;
                rzlt.PagerPageNumber = currentState.PagerState.PageNumber;
                rzlt.PagerPageSize = currentState.PagerState.PageSize;
                rzlt.PagerBaseUrl = currentState.PagerState.BaseUrl ?? baseUrl;
            }
            return rzlt;
        }

    }
}
