using DataAccess.Repositories;
using System;
using System.Collections.Generic;

namespace DataAccess.Interfaces
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll(DatabaseContext context);
        IEnumerable<T> GetWhere(DatabaseContext context, Func<T, Boolean> predicate);
        void Add(DatabaseContext context, T item);
        void Update(DatabaseContext context, T item);
        void Delete(DatabaseContext context, int id);
        void DeleteEntetiesWhere(DatabaseContext context, Func<T, bool> predicate);
    }
}
