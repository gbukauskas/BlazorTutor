using System.Reflection;
using System.Web;

namespace ClassLibraryDatabase.CustomFilter
{
    public class FilterElement : FilterModel, ICloneable
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

        public FilterElement(FilterElement? element) : base(element)
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
                IsDateValue = fldInfo.PropertyType == typeof(System.Nullable<System.DateOnly>) || fldInfo.PropertyType == typeof(System.DateOnly),
                IsIntegerValue = fldInfo.PropertyType == typeof(System.Nullable<System.Int32>) || fldInfo.PropertyType == typeof(System.Int32),
                IsSelected = isSelected,
                IsDisabled = isDisabled
            };
            return result;
        }

        public string TargetUrl
        {
            get => HttpUtility.UrlEncode($"{Name}-{Value}");
        }

        public object Clone()
        {
            return new FilterElement(this);
        }
    }
}
