using DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public abstract class RepositoryGeneric<T> where T : class
    {
        public IEnumerable<T> GetAll(DatabaseContext context)
        {
            var dbSet = context.Set<T>();

            return dbSet.ToList();
        }

        public void Add(DatabaseContext context, T item)
        {
            var dbSet = context.Set<T>();

            dbSet.Add(item);
        }

        public void Update(DatabaseContext context, T item)
        {
            var dbSet = context.Set<T>();

            dbSet.Attach(item);
            context.Entry(item).State = EntityState.Modified;
        }

        public IEnumerable<T> GetWhere(DatabaseContext context, Func<T, Boolean> predicate)
        {
            var dbSet = context.Set<T>();
            IEnumerable<T> items;

            items = dbSet.Where(predicate);
            return items.ToList();
        }

        public void Delete(DatabaseContext context, int id)
        {
            var dbSet = context.Set<T>();
            T item;

            item = dbSet.Find(id);
            if (item != null)
                dbSet.Remove(item);
        }

        public void DeleteEntetiesWhere(DatabaseContext context, Func<T, bool> predicate)
        {
            var dbSet = context.Set<T>();
            List<T> objects = new List<T>();

            if (dbSet.ToList().Any(predicate))
            {
                objects = dbSet.Where(predicate).ToList();
                foreach (T obj in objects)
                {
                    dbSet.Remove(obj);
                }
            }
        }
    }
}