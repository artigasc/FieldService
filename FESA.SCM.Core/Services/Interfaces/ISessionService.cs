namespace FESA.SCM.Core.Services.Interfaces
{
    public interface ISessionService
    {
        void AddToSession(string key, object parameter);
        string GetFromSession(string key);
        void Logout();
    }
}