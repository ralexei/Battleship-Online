using DataAccess.Interfaces;
using DataAccess.Repositories;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Interfaces
{
    public class UserService : IUserService
    {
        private readonly IPlayerRepository playerRepository;
        private readonly IGameLobbyRepository lobbyRepository;
        private readonly IDbFactory dbFactory;

        public UserService(IPlayerRepository playerRepository, IGameLobbyRepository lobbyRepository, IDbFactory dbFactory)
        {
            this.playerRepository = playerRepository;
            this.lobbyRepository = lobbyRepository;
            this.dbFactory = dbFactory;
        }

        public void AddPlayer(PlayerModel newPlayer)
        {
            using (var dbContext = new DatabaseContext())
            {
                playerRepository.Add(dbContext, newPlayer);
                dbContext.Save();
            }
        }

        public IEnumerable<PlayerModel> GetAll()
        {
            using (var dbContext = new DatabaseContext())
            {
                return playerRepository.GetAll(dbContext);
            }
        }

        public PlayerModel GetPlayerById(int id)
        {
            using (var dbContext = new DatabaseContext())
            {
                var foundPlayer = playerRepository.GetAll(dbContext).FirstOrDefault(x => x.Id == id);

                return foundPlayer;
            }
        }

        public PlayerModel GetByConnectionId(string connectionId)
        {
            using (var dbContext = new DatabaseContext())
            {
                var player = playerRepository.GetWhere(dbContext, x => x.CurrentConnectionId == connectionId).LastOrDefault();

                return player;
            }
        }


        public void AddConnection(string currentUserConnectionId, int currentUserId)
        {
            PlayerModel connectedUser;

            using (var dbContext = new DatabaseContext())
            {
                connectedUser = playerRepository.GetWhere(dbContext, x => x.Id == currentUserId).FirstOrDefault();
                playerRepository.Update(dbContext, connectedUser);
                connectedUser.CurrentConnectionId = currentUserConnectionId;
                dbContext.Save();
            }
        }

        public void EloChange(string winnerId, string loserId)
        {
            PlayerModel winner = null;
            PlayerModel loser = null;

            using (var dbContext = new DatabaseContext())
            {
                winner = playerRepository.GetWhere(dbContext, x => x.CurrentConnectionId == winnerId).ToList().LastOrDefault();
                loser = playerRepository.GetWhere(dbContext, x => x.CurrentConnectionId == loserId).ToList().LastOrDefault();
                if (winner != null && loser != null)
                {
                    double transformedRating1;
                    double transformedRating2;
                    double expectedRating;

                    playerRepository.Update(dbContext, winner);
                    transformedRating1 = Math.Pow(10, (winner.Elo / 400));
                    transformedRating2 = Math.Pow(10, (loser.Elo / 400));
                    expectedRating = transformedRating1 / (transformedRating1 + transformedRating2);
                    winner.Elo = (int)(winner.Elo + 50 * (1 - expectedRating));
                    loser.Elo = (int)(loser.Elo + 50 * (0 - expectedRating));
                }
                dbContext.Save();
            }
        }

        public void EndConnection(string connectionId)
        {
            PlayerModel disconnectedUser;

            using (var dbContext = new DatabaseContext())
            {
                disconnectedUser = playerRepository.GetWhere(dbContext, x => x.CurrentConnectionId == connectionId).FirstOrDefault();
                if (disconnectedUser != null)
                {
                    playerRepository.Update(dbContext, disconnectedUser);
                    lobbyRepository.DeleteEntetiesWhere(dbContext, x => x.Creator == connectionId);
                    disconnectedUser.CurrentConnectionId = string.Empty;
                }
                dbContext.Save();
            }
        }

        public bool ExistsConnectedWithSuchId(int id)
        {
            PlayerModel player;

            using (var dbContext = new DatabaseContext())
            {
                player = playerRepository.GetAll(dbContext).FirstOrDefault(x => x.Id == id);
            }
            if (player != null)
                return true;
            return false;
        }
    }
}
