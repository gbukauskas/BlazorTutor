using System;
using System.Collections.Generic;
using System.Text;

namespace ClassLibraryDatabase.CustomPager
{
    /// <summary>
    /// Data for printable button
    /// </summary>
    public record class PgData(int Id, string PageIndex)
    {
        //        public int Id { get; set; }

        /// <summary>
        ///    <code>string PageIndex</code> is one of listed below values:
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
        //        public string PageIndex { get; set; }

        public bool IsSelected { get; set; } = false;
        public bool IsDisabled { get; set; } = false;

        //public PgData(int pgIndex, bool isSelected = false, bool isDisabled = false)
        //{
        //    Id = pgIndex;
        //    PageIndex = pgIndex.ToString();
        //    IsSelected = isSelected;
        //    IsDisabled = isDisabled;
        //}
    }
}
