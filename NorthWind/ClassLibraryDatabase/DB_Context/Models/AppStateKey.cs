using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibraryDatabase.DB_Context.Models
{
    public class AppStateKey : IComparable
    {
        public string? AppName { get; init; }
        public string? UserId { get; init; }
        public string? ComponentName { get; init; }

        public AppStateKey(string? appName = null, string? userId = null, string? componentName = null)
        {
            AppName = appName;
            UserId = userId;
            ComponentName = componentName;
        }

        private int CompareToTyped(AppStateKey other)
        {
            String thisValue = $"{this.AppName ?? " "}_{this.UserId ?? " "}_{this.ComponentName ?? " "}";
            String otherValue = $"{other.AppName ?? " "}_{other.UserId ?? " "}_{other.ComponentName ?? " "}";
            return thisValue.CompareTo(otherValue);
        }

        public int CompareTo(object? obj)
        {
            if (obj == null)
            {
                return 1;
            }
            return (obj is not AppStateKey otherValue) ? 1 : this.CompareToTyped(otherValue);
        }
    }
}
