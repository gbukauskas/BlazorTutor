using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using WEBtransitions.ClassLibraryDatabase.DBContext;

namespace TestBusinessPart
{
    /// <summary>
    ///   <see cref="https://medium.com/bina-nusantara-it-division/a-comprehensive-guide-to-implementing-xunit-tests-in-c-net-b2eea43b48b"/>
    /// </summary>
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
