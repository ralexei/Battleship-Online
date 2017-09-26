using DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class UnitOfWork : IDatabase
    {

        private readonly IDbFactory dbFactory;
        private DatabaseContext context;

        public UnitOfWork(IDbFactory dbFactory)
        {
            this.dbFactory = dbFactory;
        }

        public DatabaseContext Context
        {
            get { return context ?? (context = dbFactory.Init()); }
        }

        public void Commit()
        {
            Context.SaveChanges();
        }
    }
}
