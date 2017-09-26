using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.Models;
using System.Data.Entity.Infrastructure;

namespace DataAccess.Repositories
{
    public class DatabaseContext : DbContext
    {
        public DbSet<GameLobby> GameLobbies { get; set; }
        public DbSet<PlayerModel> Players { get; set; }

        static DatabaseContext()
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<DatabaseContext, Migrations.Configuration>());
        }

        public DatabaseContext() : base("BattleshipContext")
        {

        }

        public virtual void Save()
        {
            SaveChanges();
        }
    }
}
