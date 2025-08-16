using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WEBtransitions.ClassLibraryDatabase.DBContext
{
    public record SelectableItem : ISelectableItem
    {
        public string ItemKey {get; set;}

        public string ItemValue { get; set; }

        public bool IsSelected { get; set; }

        public SelectableItem(string key, string value, bool isSelected = false)
        {
            this.ItemKey = key;
            this.ItemValue = value;
            this.IsSelected = isSelected;
        }
    }
}
