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
        /// Filter value (min value for dates)
        /// </summary>
        public string? Value { get; set; }

        /// <summary>
        /// Max value for dates
        /// </summary>
        public string? MaxValue { get; set; }

        /// <summary>
        /// <code>true</code> - convert string to date value before filtering
        /// </summary>
        public bool IsDateValue { get; set; } = false;

        public FilterModel(FilterElement? element)
        {
            if (element != null)
            {
                this.Name = element.Name;
                this.Value = element.Value;
                this.MaxValue = element.MaxValue;
                this.IsDateValue = element.IsDateValue;
            }
        }

        public FilterModel(Tuple<string, string, string?, bool> filterState) 
        {
            this.Name = filterState.Item1;
            this.Value = filterState.Item2;
            this.MaxValue = filterState.Item3;
            this.IsDateValue = filterState.Item4;
        }
    }

    public class FilterElement: FilterModel, ICloneable
    {
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

        public FilterElement(FilterElement? element): base(element)
        {
            if (element == null)
            {
                this.IsSelected = false;
                this.IsDisabled = false;
            }
            else
            {
                this.IsSelected = element.IsSelected;
                this.IsDisabled = element.IsDisabled;
            }
        }

        public static FilterElement CreateFilterElement(PropertyInfo fldInfo, string? value = null, string? maxValue = null,
                                                        bool isSelected = false, bool isDisabled = false) 
        {
            var result = new FilterElement(null)
            {
                Name = fldInfo.Name,
                Value = value,
                MaxValue = maxValue,
                IsDateValue = fldInfo.PropertyType == typeof(System.Nullable<System.DateOnly>),
                IsSelected = isSelected,
                IsDisabled = isDisabled
            };
            return result;
        }

        public string TargetUrl 
        {
            get =>HttpUtility.UrlEncode($"{Name}-{Value}");
        }

        public object Clone()
        {
            return new FilterElement(this);
        }
    }
}
