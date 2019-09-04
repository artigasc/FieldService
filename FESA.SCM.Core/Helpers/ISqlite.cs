using SQLite.Net.Async;

namespace FESA.SCM.Core.Helpers
{
    public interface ISqlite
    {
        SQLiteAsyncConnection GetConnection();
    }
}