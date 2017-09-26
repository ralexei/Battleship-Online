using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ClientWeb.Models;
using BusinessLogic.GameLogic;
using System.Diagnostics;
using BusinessLogic.Interfaces;
using Entities.Models;

namespace ClientWeb.Controllers
{
    public class HomeController : Controller
    {
        Field map = null;
        IUserService userService;

        public HomeController(IUserService userService)
        {
            map = new FieldDefault();
            map.SetRandomShips();
            this.userService = userService;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Game(string id, string player)
        {
            GameModel model = new GameModel()
            {
                Id = id,
                Player = player
            };
            return PartialView(model);
        }

        public ActionResult Leaderboard(int? topNr)
        {
            List<PlayerModel> players = new List<PlayerModel>();

            if (!topNr.HasValue)
                topNr = 10;
            players = userService.GetAll().OrderByDescending(x => x.Elo).Take(topNr.Value).ToList();
            return View(players);
        }
        
    }
}