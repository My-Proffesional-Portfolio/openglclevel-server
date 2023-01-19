using openglclevel_server_models.API.Security;
using openglclevel_server_models.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace openglclevel_server_security.Encryptor
{
    public class EncryptorEngine
    {

        private readonly ISecurityKeys _securityKeysValues;
        public EncryptorEngine(ISecurityKeys securityKeysValues)
        {
            _securityKeysValues = securityKeysValues;
        }

        public async Task<EncryptorResultModel> PasswordEncrypt(string password)
        {

            //string secretKey = UtilService.GetAppSettingsConfiguration("security", "passwordPrivateKey");
            string secretKey = _securityKeysValues.passwordPrivateKey;
            var saltValue = Guid.NewGuid().ToString();
            var saltBuffer = Encoding.UTF8.GetBytes(saltValue);
            byte[] clearBytes = Encoding.Unicode.GetBytes(password);

            string passwordHash = String.Empty;

            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(secretKey, saltBuffer, 1000, HashAlgorithmName.SHA256);
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);

                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        await cs.WriteAsync(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    passwordHash = Convert.ToBase64String(ms.ToArray());
                }
            }

            return new EncryptorResultModel {  PasswordHash = passwordHash, SaltValue = saltValue };

        }
    }
}
