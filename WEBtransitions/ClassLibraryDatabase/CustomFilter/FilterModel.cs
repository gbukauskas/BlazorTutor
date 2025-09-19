using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Linq;

namespace WEBtransitions.ClassLibraryDatabase.CustomFilter
{
    public class FilterModel
    {
        /// <summary>
        /// Filter on this field
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Filter value
        /// </summary>
        public string? Value { get; set; }

        /// <summary>
        /// <code>true</code> - convert string to date value before filtering
        /// </summary>
        public bool IsDateValue { get; set; } = false;

        public FilterModel(FilterElement element)
        {
            this.Name = element.Name;
            this.Value = element.Value;
            this.IsDateValue = element.IsDateValue;
        }
    }

    /*
        public class FilterArgs
        {
            public string Name { get; set; }
            public string? Value { get; set; }

            public bool IsDateValue { get; set; } = false;

            public FilterArgs(string name, string? value) 
            {
                Name = name;
                Value = value;
            }
        }
    */
    public class FilterElement : ICloneable
    {
        /// <summary>
        /// Field name
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Filter value
        /// </summary>
        public string? Value { get; set; }

        /// <summary>
        /// <code>true</code> - convert string to date value before filtering
        /// </summary>
        public bool IsDateValue { get; set; } = false;

        /// <summary>
        /// <code>true</code> - item is selected
        /// </summary>
        public bool IsSelected { get; set; } = false;

        /// <summary>
        /// <code>true</code> - item is disabled
        /// </summary>
        public bool IsDisabled { get; set; } = false;

        public string SelectedAttribute
        {
            get => this.IsSelected ? "selected" : String.Empty;
        }

        public FilterElement()
        {
        }

        public FilterElement(PropertyInfo fldInfo, string? value = null, bool isSelected = false) 
        {
            this.Name = fldInfo.Name;
            this.Value = value;
            this.IsDateValue = fldInfo.PropertyType == typeof(System.Nullable<System.DateOnly>);
            IsSelected = isSelected;
        }

        public string TargetUrl 
        {
            get =>HttpUtility.UrlEncode($"{Name}-{Value}");
        }

        public object Clone()
        {
            return new FilterElement()
            {
                Name = this.Name,
                Value = this.Value,
                IsDateValue = this.IsDateValue,
                IsSelected = this.IsSelected
            };
        }
    }
}
