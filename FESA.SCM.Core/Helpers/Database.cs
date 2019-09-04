using System;
using System.Threading;
using System.Threading.Tasks;
using FESA.SCM.Core.Models;
using Microsoft.Practices.ServiceLocation;
using SQLite.Net.Async;

namespace FESA.SCM.Core.Helpers
{
    public static class Database
    {
        private static readonly object Lock = new object();
        private static bool _initialized;
        public static SQLiteAsyncConnection Connection
        {
            get
            {
                var sqlite = ServiceLocator.Current.GetInstance<ISqlite>();
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
            typeof(User),
            typeof(Assignment),
            typeof(Activity),
            typeof(Machine),
            typeof(Contact),
            typeof(Document),
            typeof(Location),
            typeof(Trace)
        };
        private static void CreateDatabase(SQLiteAsyncConnection connection, CancellationToken cancellationToken)
        {
            lock (Lock)
            {
                connection.CreateTablesAsync(cancellationToken, TableTypes).GetAwaiter().GetResult();
            }
            _initialized = true;
        }

        public static void ClearDataBase(CancellationToken cancellationToken)
        {
            foreach (var tableType in TableTypes)
            {
                lock (Lock)
                {
                    Connection.DeleteAllAsync(tableType, cancellationToken).GetAwaiter().GetResult();
                }
            }
        }
        
    }
}