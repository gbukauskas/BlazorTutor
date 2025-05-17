using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Linq;

namespace WEBtransitions.ClassLibraryDatabase.CustomFilter
{
    public class FilterModel
    {
        public required IList<FilterElement> Filters { get; set; } 
    }

    public class FilterArgs
    {
        public string Name { get; set; }
        public string? Value { get; set; }

        public FilterArgs(string name, string? value) 
        {
            Name = name;
            Value = value;
        }
    }

    public class FilterElement : FilterArgs, ICloneable
    {
        public bool IsSelected { get; set; } = false;
        public string SelectedAttribute
        {
            get => this.IsSelected ? "selected" : String.Empty;
        }

        public FilterElement(string name = "", string? value = null, bool isSelected = false) : base(name, value)
        {
            IsSelected = isSelected;
        }

        public string TargetUrl 
        {
            get =>HttpUtility.UrlEncode($"{Name}-{Value}");
        }

        public object Clone()
        {
            return new FilterElement(this.Name, this.Value, this.IsSelected);
        }
    }
}
