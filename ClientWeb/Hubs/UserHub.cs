using BusinessLogic.Interfaces;
using Entities.Models;
using Microsoft.AspNet.SignalR;

namespace ClientWeb
{
    public class UserHub : Hub
    {
        private readonly IUserService userService;

        public UserHub(IUserService userService, ILobbyService lobbyService)
        {
            this.userService = userService;
        }

        public void AddUser(string name)
        {
            PlayerModel newPlayer;

            newPlayer = new PlayerModel { Name = name, Elo = 2000};
            userService.AddPlayer(newPlayer);
            Clients.Caller.setCookie(newPlayer.Id);
            Clients.Caller.setUser(newPlayer);
            userService.AddConnection(Context.ConnectionId, newPlayer.Id);
            Clients.Caller.setConnectedPlayerName(newPlayer.Name);
        }

        public void HasUser(int id)
        {
            PlayerModel u;

            u = userService.GetPlayerById(id);

            if (u == null)
            {
                Clients.Caller.noUser();
            }
            else
            {
                Clients.Caller.setUser(u);
                if (userService.ExistsConnectedWithSuchId(u.Id))
                {
                    userService.AddConnection(Context.ConnectionId, u.Id);
                    Clients.Caller.setConnectedPlayerName(u.Name);
                }
            }
        }
    }
}