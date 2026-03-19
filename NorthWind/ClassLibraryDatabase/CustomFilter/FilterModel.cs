namespace ClassLibraryDatabase.CustomFilter
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

        /// <summary>
        /// <code>true</code> - convert string to integer value before filtering
        /// </summary>
        public bool IsIntegerValue { get; set; } = false;

        public FilterModel(FilterElement? element)
        {
            if (element != null)
            {
                this.Name = element.Name;
                this.Value = element.Value;
                this.MaxValue = element.MaxValue;
                this.IsDateValue = element.IsDateValue;
                this.IsIntegerValue = element.IsIntegerValue;
            }
        }
    }
}
