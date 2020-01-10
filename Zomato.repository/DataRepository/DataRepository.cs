using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Zomato.DomainModel.Data;

namespace Zomato.Repository.DataRepository
{
    public class DataRepository : IDataRepository
    {
        private ApplicationDbContext _db { get; set; }

        public DataRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public DbSet<T> SetDb<T>() where T : class
        {
            DbSet<T> table = _db.Set<T>();
            return table;
        }

        public async Task AddAsync<T>(T obj) where T : class
        {
            DbSet<T> table = SetDb<T>();
            await table.AddAsync(obj);
        }

        public void Remove<T>(T obj) where T: class
        {
            DbSet<T> table = SetDb<T>();
            table.Remove(obj);
        }

        public async Task<List<T>> Get<T>() where T : class
        {
            DbSet<T> table = SetDb<T>();
            return await table.ToListAsync();
        }

        public IQueryable<T> Where<T>(Expression<Func<T, bool>> expression) where T : class
        {
            DbSet<T> table = SetDb<T>();
            return table.Where(expression);
        }

        public async Task<T> Find<T>(int obj) where T: class
        {
            DbSet<T> table = SetDb<T>();
            return await table.FindAsync(obj);
        }
    }
}
