using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using System.Collections;
using System.Diagnostics;
using System.Text.RegularExpressions;
using WEBtransitions.ClassLibraryDatabase.CustomPager;
using WEBtransitions.ClassLibraryDatabase.DBContext;
using static System.Net.Mime.MediaTypeNames;

namespace WEBtransitions.Services
{
    public class CommonSvc
    {
        const int MAX_FILESIZE = 5000 * 1024;

        /// <summary>
        /// Precision for comparision of double constatns
        /// </summary>
        public static readonly double DELTA = 0.000001;

        /// <summary>
        /// <list type="number">
        ///     <item>sortDefinionOriginal is null or empty - no sort</item>
        ///     <item>sortDefinionOriginal starts with "n" - no sort</item>
        ///     <item>sortDefinionOriginal starts with "a" - sort ascending; name of sorting field starts from SortName[2]</item>
        ///     <item>sortDefinionOriginal starts with "d" - sort descending; name of sorting field starts from SortName[2]</item>
        /// </list>
        /// </summary>
        public Tuple<string?, string> SetSort(string? sortParameter, bool setNextState)
        {
            string sortDirection;
            if (String.IsNullOrEmpty(sortParameter))
            {
                return new Tuple<string?, string>(null, "");
            }

            if (setNextState)
            {
                switch (sortParameter.Substring(0, 1))
                {
                    case "n":
                        sortDirection = "a";
                        break;
                    case "a":
                        sortDirection = "d";
                        break;
                    default:
                        sortDirection = "n";
                        break;
                }
            }
            else
            {
                sortDirection = sortParameter.Substring(0, 1);
            }
            string sortName = sortParameter.Substring(2);
            return new Tuple<string?, string>(sortDirection, sortName);
        }

        /// <summary>
        /// Fills list SortParameter with correct values for every field
        /// </summary>
        public List<string> ProcessCurrentPage(StateForComponent currentState, string[] fieldNames)
        {
            Debug.Assert(currentState != null);
            Tuple<string?, string> sortDefinition = this.SetSort(currentState.SortState, false);
            bool noSort = String.IsNullOrEmpty(sortDefinition.Item1) || String.IsNullOrEmpty(sortDefinition.Item2);
            var sortParameter = new List<string>();

            for (int i = 0; i < fieldNames.Length; i++)
            {
                if (noSort || fieldNames[i] != sortDefinition.Item2)
                {
                    sortParameter.Add($"n_{fieldNames[i]}");
                }
                else
                {
                    sortParameter.Add($"{sortDefinition.Item1}_{sortDefinition.Item2}");
                }
            }
            return sortParameter;
        }

        protected async Task<int> CountRecordsAsync(NorthwindContext ctx, string query, StateForComponent currentState)
        {
            Regex rgx = new (@"SELECT\s(?<select>.+)\sFROM", RegexOptions.IgnoreCase);

            Debug.Assert(currentState != null && currentState.PagerState != null);
            try
            {
                //string countQuery = query.Replace("*", "COUNT(1) AS Value");    //TODO: replace with regex

                string countQuery = rgx.Replace(query, "SELECT COUNT(1) AS Value FROM");
                currentState.PagerState.RowCount = await ctx.Database.SqlQueryRaw<int>(countQuery).FirstOrDefaultAsync();

                int pgCount = currentState.PagerState.RowCount / currentState.PagerState.PageSize;
                if (currentState.PagerState.RowCount % currentState.PagerState.PageSize > 0)
                {
                    pgCount += 1;
                }
                currentState.PagerState.PageCount = pgCount;
                return currentState.PagerState.RowCount;
            }
            catch (Exception ex)
            {
                string s = ex.Message;
                throw;
            }
        }

        public int GetPGButtonCount(IConfiguration? configuration, string pageName)
        {
            int maxButtons = 5;
            if (configuration != null)
            {
                string? maxButtonsStr = configuration[$"AppSettings:maxButtons:{pageName}"] ?? configuration["AppSettings:maxButtons:AnyPage"];
                if (!int.TryParse(maxButtonsStr ?? "5", out maxButtons))
                {
                    maxButtons = 5;
                }
            }
            return maxButtons;
        }

        public int GetDefaultPageSize(IConfiguration? configuration, string pageName)
        {
            int defaultPageSize = 9;
            if (configuration != null)
            {
                string? pgSizeStr = configuration[$"AppSettings:defaultPgSize:{pageName}"] ?? configuration["AppSettings:defaultPgSize:AnyPage"];
                if (!int.TryParse(pgSizeStr ?? "9", out defaultPageSize))
                {
                    defaultPageSize = 9;
                }
            }
            return defaultPageSize;
        }

        public AppState CreateNewState(AppStateKey stateId, int buttonCount, int defaultPageSize, string pagerBaseUrl)
        {
            Debug.Assert(!String.IsNullOrEmpty(stateId.AppName) && !String.IsNullOrEmpty(stateId.UserId) && !String.IsNullOrEmpty(stateId.ComponentName));
            var newState = new AppState()
            {
                AppName = stateId.AppName,
                UserId = stateId.UserId,
                ComponentName = stateId.ComponentName,
                SortState = "",
                FilterFieldName = "",
                FilterFieldValue = "",
                FilterFieldMaxValue = "",
                FilterIsDateValue = (byte)0,
                PagerButtonCount = buttonCount,
                PagerRowCount = 0,      // The application will set actual value after reading dbo.Employees table
                PagerPageCount = 0,     // The application will set actual value after reading dbo.Employees table
                PagerPageNumber = 1,    // Display all records without paging
                PagerPageSize = defaultPageSize,
                PagerBaseUrl = pagerBaseUrl,
                IsDeleted = (byte)0,
                LastInsertedId = "-1"   // The application has no information about insertion
            };
            return newState;
//            return await StateManager.CreateEntity(newState);
        }

        public async Task<byte[]?> BytesFromStream(IBrowserFile? PhotoFile)
        {
            if (PhotoFile != null && PhotoFile.Size > 0)
            {
                var fileStream = PhotoFile.OpenReadStream(MAX_FILESIZE);
                using MemoryStream ms = new();
                await fileStream.CopyToAsync(ms);
                return ms.ToArray();
            }
            else
            {
                return null;
            }
        }
    }
}

