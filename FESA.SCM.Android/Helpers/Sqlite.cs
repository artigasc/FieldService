using System;
using System.IO;
using FESA.SCM.Core.Helpers;
using FESA.SCM.Core.Models;
using SQLite.Net;
using SQLite.Net.Async;
using SQLite.Net.Platform.XamarinAndroid;

namespace FESA.SCM.Android.Helpers
{
    public class Sqlite : ISqlite
    {
        public SQLiteAsyncConnection GetConnection()
        {
            var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var path = Path.Combine(documentsPath, Constants.DatabaseName);

            if (!File.Exists(path)) File.Create(path);

            var platform = new SQLitePlatformAndroid();
            var connectionString = new SQLiteConnectionString(path, storeDateTimeAsTicks: false);
            var connection = new SQLiteAsyncConnection(() => new SQLiteConnectionWithLock(platform, connectionString));

            return connection;
        }
    }
}