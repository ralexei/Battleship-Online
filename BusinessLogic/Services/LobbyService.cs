using BusinessLogic.Interfaces;
using System.Collections.Generic;
using Entities.Models;
using DataAccess.Interfaces;
using DataAccess.Repositories;

namespace BusinessLogic.Services
{
    public class LobbyService : ILobbyService
    {
        private readonly IPlayerRepository players;
        private readonly IGameLobbyRepository lobbies;
        private readonly IDbFactory dbFactory;

        public LobbyService(IPlayerRepository players, IGameLobbyRepository lobbies, IDbFactory dbFactory)
        {
            this.players = players;
            this.lobbies = lobbies;
            this.dbFactory = dbFactory;
        }

        public void CreateLobby(GameLobby newLobby)
        {
            using (var dbContext = new DatabaseContext())
            {
                lobbies.Add(dbContext, newLobby);
                dbContext.Save();
            }
        }

        public IEnumerable<GameLobby> GetAllLobbies()
        {
            using (var dbContext = new DatabaseContext())
            {
                return lobbies.GetAll(dbContext);
            }
        }

        public void RemoveLobbiesByConnectionId(string connectionId)
        {
            using (var dbContext = new DatabaseContext())
            {
                lobbies.DeleteEntetiesWhere(dbContext, x => x.Creator == connectionId);
                dbContext.Save();
            }
        }
    }
}
