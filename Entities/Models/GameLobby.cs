using System;

namespace Entities.Models
{
    public class GameLobby
    {
        public Int32 Id { get; set; }
        public string LobbyName { get; set; }
        public string Password { get; set; }
        public string Creator { get; set; }
        public Int32 CreatorElo { get; set; }
    }
}
