using System;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using PCLCrypto;
using Plugin.Settings;

namespace FESA.SCM.Core.Services.Implementation
{
    public class Encryption
    {
        public byte[] CreateSalt(int lengthInBytes)
        {
            var key = lengthInBytes.ToString();
            byte[] salt;
            var saltString = CrossSettings.Current.GetValueOrDefault(key, string.Empty);
            if (!string.IsNullOrEmpty(saltString))
            {
                salt = Convert.FromBase64String(saltString);
            }
            else
            {
                salt = WinRTCrypto.CryptographicBuffer.GenerateRandom(lengthInBytes);
                var value = Convert.ToBase64String(salt);
                CrossSettings.Current.AddOrUpdateValue(key, value);
            }
            return salt;
        }

        public byte[] CreateDerivedKey(string password, byte[] salt, int keyLengthInBytes = 32, int iterations = 1000)
        {
            return NetFxCrypto.DeriveBytes.GetBytes(password, salt, iterations, keyLengthInBytes);
        }

        public string EncryptAes(string data, string password, byte[] salt)
        {
            var key = CreateDerivedKey(password, salt);
            var aes = WinRTCrypto.SymmetricKeyAlgorithmProvider.OpenAlgorithm(SymmetricAlgorithm.AesCbcPkcs7);
            var symetricKey = aes.CreateSymmetricKey(key);
            var encryptedBytes = WinRTCrypto.CryptographicEngine.Encrypt(symetricKey, Encoding.UTF8.GetBytes(data));
            return Convert.ToBase64String(encryptedBytes);
        }
        public byte[] DecryptAes(string data, string password, byte[] salt)
        {
            var key = CreateDerivedKey(password, salt);
            var aes = WinRTCrypto.SymmetricKeyAlgorithmProvider.OpenAlgorithm(SymmetricAlgorithm.AesCbcPkcs7);
            var symetricKey = aes.CreateSymmetricKey(key);
            var encryptedBytes = Convert.FromBase64String(data);
            return WinRTCrypto.CryptographicEngine.Decrypt(symetricKey, encryptedBytes);
        }

        public string EncryptObjectAes(object data, string password, byte[] salt)
        {
            var key = CreateDerivedKey(password, salt);


            var aes = WinRTCrypto.SymmetricKeyAlgorithmProvider.OpenAlgorithm(SymmetricAlgorithm.AesCbcPkcs7);
            var symetricKey = aes.CreateSymmetricKey(key);
            var serializer = new DataContractSerializer(typeof(object));
            byte[] byteArr;
            using (var ms = new MemoryStream())
            {
                serializer.WriteObject(ms, data);
                byteArr = ms.ToArray();
            }
            var encriptedBytes = WinRTCrypto.CryptographicEngine.Encrypt(symetricKey, byteArr);
            return Convert.ToBase64String(encriptedBytes);
        }
    }
}