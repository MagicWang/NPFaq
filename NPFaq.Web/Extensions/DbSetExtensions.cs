using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace NPFaq.Web.Extensions
{
    public static class DbSetExtensions
    {
        public static void AddRange<TEntity>(this DbSet<TEntity> set, IEnumerable<TEntity> entities) where TEntity : class
        {
            if (set != null && entities != null)
                entities.ToList().ForEach(l => set.Add(l));
        }
    }
}