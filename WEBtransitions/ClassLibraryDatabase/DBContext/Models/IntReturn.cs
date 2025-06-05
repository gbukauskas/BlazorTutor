using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBtransitions.ClassLibraryDatabase.DBContext;

namespace WEBtransitions.ClassLibraryDatabase.DBContext;
public class IntReturn
{
    public int Value { get; set; }

    internal static void Configure(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<IntReturn>(entity => entity.HasNoKey());
        modelBuilder.Ignore<IntReturn>();
    }
}
