using System.IO;
using System.Runtime.Serialization;
using System.Text;
using FESA.SCM.Core.Models;
using FESA.SCM.Core.Services.Interfaces;
using Microsoft.Practices.ServiceLocation;
using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace FESA.SCM.Core.Services.Implementation
{
    public class SessionService : ISessionService
    {
        private readonly ISettings _settings;
        private readonly Encryption _encryptionService;
        private readonly byte[] _salt;
        private static readonly string SettingsDefault = string.Empty;

        public SessionService()
        {
            _settings = CrossSettings.Current;
            _encryptionService = new Encryption();
            _salt = _encryptionService.CreateSalt(16);
        }

        public Session GetSession()
        {
            var session = new Session
            {
                AccessToken = LoadEncryptedSettingValue("session_token"),
                Id = LoadEncryptedSettingValue("session_id"),
                UserName = LoadEncryptedSettingValue("session_username"),
                UserId = LoadEncryptedSettingValue("session_userId"),
                Photo = LoadEncryptedSettingValue("session_userphoto"),
                Email = LoadEncryptedSettingValue("session_email")
            };

            return session;
        }

        public void SaveSession(Session session)
        {
            SaveEncryptedSettingValue("session_token", session.AccessToken);
            SaveEncryptedSettingValue("session_id", session.Id);
            SaveEncryptedSettingValue("session_username", session.UserName);
            SaveEncryptedSettingValue("session_userId", session.UserId);
            SaveEncryptedSettingValue("session_userphoto", session.Photo);
            SaveEncryptedSettingValue("session_email", session.Email);
        }

        public void Logout()
        {
            RemoveFromSession("technician");
            RemoveFromSession("logged");
            ServiceLocator.Current.GetInstance<IIdentityService>().LogOffAsync();
        }

        public void AddToSession(string key, object parameter)
        {
            var value = parameter as string;
            if (value != null)
                SaveEncryptedSettingValue(key, value);
            else
                SaveEncryptedObjectValue(key, parameter);
        }

        public string GetFromSession(string key)
        {
            return LoadEncryptedSettingValue(key);
        }

        public object GetObjectFromSession(string key)
        {
            return LoadEncryptedObjectValue(key);
        }

        public void RemoveFromSession(string key)
        {
            _settings.Remove(key);
        }
        #region Private Methods
        private string LoadEncryptedSettingValue(string key)
        {
            var encryptedBytes = _settings.GetValueOrDefault(key, SettingsDefault);
            if (encryptedBytes.Equals(SettingsDefault)) return string.Empty;
            var valueBytes = _encryptionService.DecryptAes(encryptedBytes, Constants.FerreyrosEncryptKey, _salt);
            var value = Encoding.UTF8.GetString(valueBytes, 0, valueBytes.Length);
            return value;
        }

        private object LoadEncryptedObjectValue(string key)
        {
            var encryptedBytes = _settings.GetValueOrDefault(key, SettingsDefault);
            if (encryptedBytes.Equals(SettingsDefault)) return string.Empty;
            var valueBytes = _encryptionService.DecryptAes(encryptedBytes, Constants.FerreyrosEncryptKey, _salt);
            var serializer = new DataContractSerializer(typeof(object));
            object value;
            using (var ms = new MemoryStream(valueBytes))
            {
                value = serializer.ReadObject(ms);
            }
            return value;
        }

        private void SaveEncryptedObjectValue(string key, object value)
        {
            if (string.IsNullOrWhiteSpace(key) || value == null) return;
            var encryptData = _encryptionService.EncryptObjectAes(value, Constants.FerreyrosEncryptKey, _salt);
            _settings.AddOrUpdateValue(key, encryptData);
        }

        private void SaveEncryptedSettingValue(string key, string value)
        {
            if (string.IsNullOrWhiteSpace(key) || string.IsNullOrWhiteSpace(value)) return;
            var protectedValues = _encryptionService.EncryptAes(value, Constants.FerreyrosEncryptKey, _salt);
            _settings.AddOrUpdateValue(key, protectedValues);
        }
        #endregion
    }
}