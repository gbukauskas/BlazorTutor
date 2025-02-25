using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using WEBtransitions.ClassLibraryDatabase.DBContext;

namespace TestBusinessPart
{
    public class DatabaseFixture : IDisposable
    {
        // Replace these strings with your paths to the original database and the current copy of a database
        private const string dbOrigPath = @"D:\darbas\BlazorTutor\WEBtransitions\WEBtransitions\DB\northwind.db";
        private const string dbCopyPath = @"D:\tmp\NorthWindCopy.db";

        public NorthwindContext DbContext
        {
            get;
            private set;
        }


        public DatabaseFixture()
        {
            if (File.Exists(dbCopyPath))
            {
                File.Delete(dbCopyPath);
            }
            File.Copy(dbOrigPath, dbCopyPath);
            DbContext = NorthwindContext.CreateDBcontext(dbCopyPath);
        }

        public void Dispose()
        {
            DbContext.Dispose();
        }
    }
}
