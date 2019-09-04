using SQLite.Net.Async;

namespace FESA.SCM.Web.Helpers
{
    public interface ISqlLite
    {
        SQLiteAsyncConnection GetConnection();
    }
}