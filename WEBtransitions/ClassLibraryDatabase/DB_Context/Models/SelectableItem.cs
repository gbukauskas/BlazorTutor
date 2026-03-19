using ClassLibraryDatabase.WEBtransitions_DBContext.Interfaces;

namespace ClassLibraryDatabase.WEBtransitions_DBContext.Models
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
