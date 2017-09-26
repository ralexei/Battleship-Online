using DataAccess.Interfaces;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class PlayerRepository : RepositoryGeneric<PlayerModel>, IPlayerRepository
    {

    }

    public interface IPlayerRepository : IRepository<PlayerModel>
    { }
}
