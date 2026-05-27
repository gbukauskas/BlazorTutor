using System;
using System.Collections.Generic;
using System.Text;

namespace ClassLibraryDatabase.CustomFilter
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    internal class AllowFilteringAttribute: Attribute
    {
    }
}
