using NHibernate;
using System.Collections.Generic;
using System.Linq;

namespace Utilities
{
    public static class DatabaseUpdater
    {
        public static string GetSqlDelete(string tableName)
        {
            return $"delete from {tableName}";
        }

        public static void ExecuteSqlUpdate(this ISession session, string sqlUpdateQuery)
        {
            session.CreateSQLQuery(sqlUpdateQuery).ExecuteUpdate();
        }

        public static void ClearAllTables(this ISession session, IEnumerable<string> tableNames)
        {
            var updateQueries = tableNames.Select(GetSqlDelete).ToList();
            updateQueries.ForEach(q => ExecuteSqlUpdate(session, q));
        }
    }
}
