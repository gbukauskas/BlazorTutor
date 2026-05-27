using ClassLibraryDatabase.DB_Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace ClassLibraryDatabase.Services
{
    public class CommonSvc
    {
        const int MAX_FILESIZE = 5000 * 1024;

        /// <summary>
        /// Precision for comparision of double constatns
        /// </summary>
        public static readonly double DELTA = 0.000001;

        protected NorthwindContext? _ctx = null;
        private IDbContextFactory<NorthwindContext>? factory = null;

        public NorthwindContext Ctx
        {
            get
            {
                Debug.Assert(this.factory != null || _ctx != null);
                if (_ctx == null)
                {
                    _ctx = factory!.CreateDbContext();
                }
                return _ctx!;
            }
            set
            {
                Debug.Assert(value != null);
                _ctx = value;
            }
        }

        public CommonSvc(IDbContextFactory<NorthwindContext>? factory)
        {
            if (factory != null)
            {
                this.factory = factory;
            }
        }

        public void SetDbContext(NorthwindContext ctx)
        {
            this._ctx = ctx;
        }

    }
}
