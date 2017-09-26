using Entities.Models;
using System.Collections.Generic;

namespace BusinessLogic.Interfaces
{
    public interface IUserService
    {
        IEnumerable<PlayerModel> GetAll();
        PlayerModel GetByConnectionId(string connectionId);
        PlayerModel GetPlayerById(int id);
        void AddPlayer(PlayerModel newPlayer);
        void AddConnection(string currentUserConectionId, int currentUserId);
        void EndConnection(string connectionId);
        void EloChange(string winnerId, string loserId);
        bool ExistsConnectedWithSuchId(int id);
    }
}