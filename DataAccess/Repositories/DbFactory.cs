using DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class DbFactory : Disposable, IDbFactory
    {
        DatabaseContext context;

        public DatabaseContext Init()
        {
            return context ?? (context = new DatabaseContext());
        }
    }
}
