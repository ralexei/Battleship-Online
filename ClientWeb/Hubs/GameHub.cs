using BusinessLogic.GameLogic;
using BusinessLogic.Interfaces;
using Entities.Models;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace ClientWeb
{
    public class GameHub : Hub
    { 
        public static Dictionary<string, MyGame> games = new Dictionary<string,MyGame>();
        private readonly ILobbyService lobbyService;
        private readonly IUserService userService;

        public GameHub(IUserService userService, ILobbyService lobbyService)
        {
            this.userService = userService;
            this.lobbyService = lobbyService;
        }

        public void StartGame(string roomId, string player)
        {    
            Room room = LobbyHub.Rooms.Find(r => r.RoomId == roomId);

            if (room != null)
            {
                MyGame game = new MyGame()
                {
                    Players = new Player[2] { room.Player1, room.Player2 }
                };
                game.Start();
                games[room.RoomId] = game;

                LobbyHub.Rooms.Remove(room);
                Clients.Caller.setId(Context.ConnectionId);
            }
            else
            {
                if (!games.ContainsKey(roomId))
                    Clients.Caller.noGame();
                else
                    Clients.Caller.setId(Context.ConnectionId);
            }
        }

        public void GameOver(string winnerId, string loserId)
        {
            userService.EloChange(winnerId, loserId);
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            String gameId = null;

            foreach (var iter in games)
            {
                int i = iter.Value.HasPlayer(Context.ConnectionId);
                if (i >= 0)
                {
                    int o = i == 1 ? 0 : 1;
                    gameId = iter.Key;
                    if (iter.Value.Players[o] != null)
                    {
                        try
                        {
                            Clients.Client(iter.Value.Players[o].Id).endGame(1, iter.Value.Players[o == 1 ? 0 : 1].Id);
                            Clients.Client(iter.Value.Players[o == 1 ? 0 : 1].Id).endGame(0, iter.Value.Players[o].Id);
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine("GameHub OnDisconnected:");
                            Debug.WriteLine(ex);
                        }
                    }
                }
            }
            if (gameId != null)
                games.Remove(gameId);
            return base.OnDisconnected(stopCalled);
        }

        public void TakeShoot(string gameId, string player, Point p)
        {  
            if (games.ContainsKey(gameId))
            {
                MyGame g = games[gameId];
                if (g.Current_Player().Name == player && g.IsNotGameOver())
                {
                    g.TakeShoot(p);
                    try {
                        Clients.Client(g.Players[0].Id).updateInfo();
                        Clients.Client(g.Players[1].Id).updateInfo();
                        if (!g.IsNotGameOver())
                        {
                            Clients.Client(g.Players[0].Id).endGame(g.CurrentPlayer == 0 ? 1 : 0, g.Players[1].Id);
                            Clients.Client(g.Players[1].Id).endGame(g.CurrentPlayer == 1 ? 1 : 0, g.Players[0].Id);
                        }
                    }
                    catch(Exception ex)
                    {
                        Debug.WriteLine("Take shoot");
                        Debug.WriteLine(ex);
                    }
                }
            }
            else
            {
                Clients.Caller.noGame();
            }
        }

        public void GetInfo(string gameId, string connectionId, string player)
        {
            if(games.ContainsKey(gameId))
            {
                MyGame g = games[gameId];
                Field my = null;
                List<Ship> deadShips = null;
                CellStatus[,] op = null;
                bool wait = true;

                if(g.Players[0].Name == player)
                {
                    g.Players[0].Id = connectionId;
                    my = g.GetMap(0);
                    op = g.GetCellStatus(1);
                    deadShips = g.GetDeadShips(1);
                    wait = 1 == g.CurrentPlayer;
                }
                else if (g.Players[1].Name == player)
                {
                    g.Players[1].Id = connectionId;
                    my = g.GetMap(1);
                    op = g.GetCellStatus(0);
                    deadShips = g.GetDeadShips(0);
                    wait = 0 == g.CurrentPlayer;
                }
                Clients.Client(connectionId).getInfo(my, op, deadShips, wait);
            }
            else
            {
                Clients.Caller.noGame();
            }
        }

        public void Connect()
        {
            Cookie cookie;

            if (Context.RequestCookies.TryGetValue("id", out cookie))
            {
                int idFromCookie;

                if (int.TryParse(cookie.Value, out idFromCookie))
                {
                    PlayerModel connectedPlayer;

                    connectedPlayer = userService.GetPlayerById(idFromCookie);
                    userService.AddConnection(Context.ConnectionId, idFromCookie);
                }
            }
        }
    } 
}