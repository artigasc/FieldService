using System;
using System.IO;
using FESA.SCM.Core.Helpers;
using FESA.SCM.Core.Models;
using SQLite.Net;
using SQLite.Net.Async;
using SQLite.Net.Platform.XamarinIOS;

namespace FESA.SCM.iPhone.Helpers
{
    public class Sqlite : ISqlite
    {
        public SQLiteAsyncConnection GetConnection()
        {
            var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var library = Path.Combine(documentsPath, "..", "Library");
            var path = Path.Combine(library, Constants.DatabaseName);

            if (!File.Exists(path)) File.Create(path);

            var platform = new SQLitePlatformIOS();
            var connectionString = new SQLiteConnectionString(path, storeDateTimeAsTicks: false);
            var connection = new SQLiteAsyncConnection(() => new SQLiteConnectionWithLock(platform, connectionString));

            return connection;
        }
    }
}