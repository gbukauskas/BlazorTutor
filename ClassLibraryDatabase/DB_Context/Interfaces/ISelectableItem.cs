using System;
using System.Collections.Generic;
using System.Text;

namespace ClassLibraryDatabase.DB_Context.Interfaces
{
    public interface ISelectableItem
    {
        public string ItemKey { get; }
        public string ItemValue { get; }
    }
}
