using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Diagnostics;
using WEBtransitions.ClassLibraryDatabase.CustomPager;
using WEBtransitions.ClassLibraryDatabase.DBContext;
using static System.Net.Mime.MediaTypeNames;

namespace WEBtransitions.Services
{
    public class CommonSvc
    {
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
            Debug.Assert(currentState != null && currentState.PagerState != null);
            try
            {
                string countQuery = query.Replace("*", "COUNT(1) AS Value");
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

        /*
                public string GetImageURL(byte[] img)
                {
                    var b64String = Convert.ToBase64String(img);
                    return $"data:image/png;base64{b64String}";


            https://www.telerik.com/forums/display-image-stored-in-sql-db     
            https://stackoverflow.com/questions/72114518/show-an-image-stream-to-client-in-blazor-server-without-javascript

                    Image image;
                    using (MemoryStream ms = new MemoryStream(img))
                    {
                        ms.Position = 0;
                        image = Image.FromStream(ms);
                    }
                    Image thumb = image.GetThumbnailImage(64, 64, () => false, IntPtr.Zero);
                    ImageConverter ic = new ImageConverter();
                    byte[] thumbBytes = (byte[])ic.ConvertTo(thumb, typeof(byte[]));

                    string img64 = Convert.ToBase64String(thumbBytes);
                    string urlData = string.Format("data:image/jpg;base64, {0}", img64);
                    return urlData;

                }
        */
    }
}

