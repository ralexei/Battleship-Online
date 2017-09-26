using Microsoft.AspNet.SignalR;
using BusinessLogic.GameLogic;
using System.Diagnostics;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using BusinessLogic.Interfaces;
using Entities.Models;
using System.Linq;

namespace ClientWeb
{
    public class LobbyHub : Hub
    {
        public static List<Room> Rooms = new List<Room>();
        Field field;

        private readonly IUserService userService;
        private readonly ILobbyService lobbyService;

        public LobbyHub(IUserService userService, ILobbyService lobbyService)
        {
            field = new FieldDefault();
            this.userService = userService;
            this.lobbyService = lobbyService;
        }

        public override Task OnConnected()
        {
            Clients.Caller.getUserId(Context.ConnectionId);
            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            lobbyService.RemoveLobbiesByConnectionId(Context.ConnectionId);
            Clients.All.getLobbies(lobbyService.GetAllLobbies().OrderByDescending(x => x.CreatorElo));
            return base.OnDisconnected(stopCalled);
        }

        public void Connect()
        {
            IEnumerable<GameLobby> lobbies;

            lobbies = lobbyService.GetAllLobbies();
            Clients.Caller.getLobbies(lobbies.OrderByDescending(x => x.CreatorElo));
        }

        public void Play(string player1, string player2, string checkId)
        {
            List<GameLobby> lobbies = lobbyService.GetAllLobbies().ToList();
            GameLobby lobby = lobbies.Find(x => x.Creator == player2);

            if (lobby != null)
            {
                if (string.IsNullOrEmpty(lobby.Password))
                {
                    Rooms.Add(new Room(player1, player2));
                    Clients.Client(player1).getMap();
                    Clients.Client(player2).getMap();
                }
                else
                {
                    Clients.Client(player1).pass(lobby, 0);
                }
            }
        }

        public void Pass(string player1, string player2, string pass)
        {
            List<GameLobby> lobbies = lobbyService.GetAllLobbies().ToList();
            GameLobby lobby = lobbies.Find(l => l.Creator == player2);

            if (lobby != null)
            {
                if (lobby.Password == pass && pass != null)
                {
                    Rooms.Add(new Room(player1, player2));
                    Clients.Client(player1).getMap();
                    Clients.Client(player2).getMap();
                }
                else
                {
                    Clients.Client(player1).pass(lobby, -1);
                }
            }
        }

        public void GetMap(string player, List<Ship> list)
        {
            Room room = Rooms.Find(r => r.HasPlayer(player));
            Field field = new FieldDefault();

            field.SetShipsList(list);
            room.SetMap(player, field);
            if(room.IsReady())
            {
                Clients.Client(room.Player1.Name).play(room.RoomId);
                Clients.Client(room.Player2.Name).play(room.RoomId);         
            }
        }

        public void GetRandShips(Field field)
        {
            field.SetRandomShips();
            Clients.Caller.setShips(field);
        }

        public void GetField()
        {
            field.SetRandomShips();
            Clients.Caller.initField(field.Ships);
        }

        public void AddLobby(string name, string pass)
        {
            GameLobby newLobby;
            PlayerModel creator;
            List<GameLobby> lobbies;

            creator = userService.GetByConnectionId(Context.ConnectionId);
            newLobby = new GameLobby { LobbyName = name, Password = pass, Creator = creator.CurrentConnectionId };
            newLobby.CreatorElo = creator.Elo;
            lobbyService.CreateLobby(newLobby);
            lobbies = lobbyService.GetAllLobbies().OrderByDescending(x => x.CreatorElo).ToList();
            Clients.All.getLobbies(lobbies);
        }
    }
}