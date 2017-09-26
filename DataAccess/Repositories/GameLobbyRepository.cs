using DataAccess.Interfaces;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class GameLobbyRepository : RepositoryGeneric<GameLobby>, IGameLobbyRepository
    {

    }

    public interface IGameLobbyRepository : IRepository<GameLobby>
    { }
}
