using System;
using System.Linq;
using RespawnCoreApiExample.Domain.Db;

namespace RespawnCoreApiExample.DataAccess.Extensions
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> GetById<T>(this IQueryable<T> queryable, Guid id) where T : BaseEntity
        {
            return queryable.Where(e => e.Id == id);
        }

        public static IQueryable<T> GetByName<T>(this IQueryable<T> queryable, string name) where T : INamed
        {
            return queryable.Where(e => e.Name == name);
        }
    }
}