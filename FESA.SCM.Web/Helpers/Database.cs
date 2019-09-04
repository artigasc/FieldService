using FESA.SCM.Web.Models;
using System;
using Microsoft.Practices.ServiceLocation;
using SQLite.Net.Async;
using System.Threading;

namespace FESA.SCM.Web.Helpers
{
    public static class Database
    {
        private static readonly object Lock = new object();
        private static bool _initialized;
        public static SQLiteAsyncConnection Connection
        {
            get
            {
                var sqlite = ServiceLocator.Current.GetInstance<ISqlLite>();
                var connection = sqlite.GetConnection();
                if (!_initialized)
                {
                    CreateDatabase(connection, new CancellationToken());
                }
                return connection;
            }
        }

        private static readonly Type[] TableTypes = new Type[]
        {
            typeof(User)
            
        };
        private static void CreateDatabase(SQLiteAsyncConnection connection, CancellationToken cancellationToken)
        {
            lock (Lock)
            {
                connection.CreateTablesAsync(cancellationToken, TableTypes).GetAwaiter().GetResult();
            }
            _initialized = true;
        }

    }
}