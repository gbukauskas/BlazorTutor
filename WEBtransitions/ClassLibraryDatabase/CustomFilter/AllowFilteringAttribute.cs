﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WEBtransitions.ClassLibraryDatabase.CustomFilter
{
    [AttributeUsage(AttributeTargets.Property, Inherited =false, AllowMultiple = false)]
    public class AllowFilteringAttribute: Attribute
    {
    }
}
