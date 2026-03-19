using System;
using System.Collections.Generic;
using System.Text;

namespace ClassLibraryDatabase.CustomFilter
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public class AllowFilteringAttribute: Attribute
    {
    }
}
