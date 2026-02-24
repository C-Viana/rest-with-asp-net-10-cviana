using Microsoft.EntityFrameworkCore;
using rest_with_asp_net_10_cviana.Models;
using rest_with_asp_net_10_cviana.Models.Base;
using rest_with_asp_net_10_cviana.Models.Context;

namespace rest_with_asp_net_10_cviana.Repositories
{
    public class GenericRepository<T>(MSSQLContext context) : IRepository<T> where T : BaseEntity
    {
        protected readonly MSSQLContext _context = context;
        private readonly DbSet<T> _dataSet = context.Set<T>();

        T IRepository<T>.Create(T entity)
        {
            _context.Add(entity);
            _context.SaveChanges();
            return entity;
        }

        void IRepository<T>.Delete(long id)
        {
            T PersistedEntity = _dataSet.Find(id);
            if (PersistedEntity == null) return;

            _context.Remove(PersistedEntity);
            _context.SaveChanges();
        }

        bool IRepository<T>.Exists(long id)
        {
            return _dataSet.Any(e => e.Id == id);
        }

        List<T> IRepository<T>.FindAll()
        {
            return [.. _dataSet];
        }

        T IRepository<T>.FindById(long id)
        {
            return _dataSet.Find(id);
        }

        T IRepository<T>.Update(T entity)
        {
            T PersistedEntity = _dataSet.Find(entity.Id);
            if (PersistedEntity == null) return null;

            _context.Entry(PersistedEntity).CurrentValues.SetValues(entity);
            _context.SaveChanges();
            return entity;
        }

        public List<T> FindWithPagedSearch(string query)
        {
            return [.. _dataSet.FromSqlRaw(query)];
        }

        public int GetCount(string query)
        {
            using var connection = _context.Database.GetDbConnection();
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandText = query;

            var result = command.ExecuteScalar();

            return Convert.ToInt32(result);
        }

    }
}
