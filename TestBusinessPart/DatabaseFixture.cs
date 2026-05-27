using ClassLibraryDatabase.DB_Context;
using Microsoft.Data.Sqlite;
using SQLitePCL;
using System;
using System.Collections.Generic;
using System.Text;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace TestBusinessPart
{
    /// <summary>
    ///   <see cref="https://medium.com/bina-nusantara-it-division/a-comprehensive-guide-to-implementing-xunit-tests-in-c-net-b2eea43b48b"/>
    /// </summary>
    public class DatabaseFixture : IDisposable
    {
        // Replace these strings with your paths to the original database and the current copy of a database
        private const string dbOrigPath = @"D:\darbas\BlazorTutor\FluentUiNorthWind\DB\northwind - Copy.db";
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

            Batteries.Init();
            DbContext = NorthwindContext.CreateDBcontext(dbCopyPath);
        }

        public void Dispose()
        {
            if (DbContext != null)
            {
                DbContext.Dispose();
            }
        }
    }
}
