using Ninject.Modules;
using DataAccess.Repositories;
using DataAccess.Interfaces;
using Ninject.Web.Common;

namespace BusinessLogic
{
    public class RepositoryModule : NinjectModule
    {
        public override void Load()
        {
            //Repositories
            Bind<IPlayerRepository>().To<PlayerRepository>();
            Bind<IGameLobbyRepository>().To<GameLobbyRepository>();

            Bind<IDbFactory>().To<DbFactory>().InSingletonScope();
        }
    }
}