using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WEBtransitions.ClassLibraryDatabase.CustomPager
{
    /// <summary>
    /// Data for printable button
    /// </summary>
    public class PgData
    {
        public int Id { get; set; }

        /// <summary>
        ///     <list type="number">
        ///         <item>
        ///             <term>L</term>
        ///             <description>First page</description>
        ///         </item>
        ///         <item>
        ///             <term>l</term>
        ///             <description>Previous or first page</description>
        ///         </item>
        ///         <item>
        ///             <term>r</term>
        ///             <description>Next or last page</description>
        ///         </item>
        ///         <item>
        ///             <term>R</term>
        ///             <description>Last page</description>
        ///         </item>
        ///         <item>
        ///             <term><code>Id</code></term>
        ///             <description>Go to page which number is defined in the Id property</description>
        ///         </item>
        ///     </list>
        /// </summary>
        public string PageIndex { get; set; }
        public bool IsSelected { get; set; }
        public bool IsDisabled { get; set; }

        public PgData(int pgIndex, bool isSelected = false, bool isDisabled = false)
        {
            Id = pgIndex;
            PageIndex = pgIndex.ToString();
            IsSelected = isSelected;
            IsDisabled = isDisabled;
        }
    }
}
