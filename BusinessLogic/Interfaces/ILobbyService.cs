using Entities.Models;
using System.Collections.Generic;

namespace BusinessLogic.Interfaces
{
    public interface ILobbyService
    {
        IEnumerable<GameLobby> GetAllLobbies();
        void CreateLobby(GameLobby newLobby);
        void RemoveLobbiesByConnectionId(string connectionId);
    }
}