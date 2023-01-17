using openglclevel_server_data.DataAccess;
using openglclevel_server_infrastructure.Repositories.Interfaces;
using openglclevel_server_infrastructure.Services;
using openglclevel_server_models.API.Security;
using openglclevel_server_models.Requests.Accounts;
using openglclevel_server_models.Security;
using openglclevel_server_security.Encryptor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace openglclevel_server_backend.Services
{
    public class UserService : IUserService
    {

        private readonly ISecurityKeys _securityKeysValues;
        private readonly EncryptorEngine _encryptor;
        private readonly IUserRepository _userRepository;
        private readonly OpenglclevelContext _dbContext;
        public UserService(ISecurityKeys securityKeysValues, OpenglclevelContext dbContext,
            EncryptorEngine encryptor, IUserRepository userRepository)
        {
            _securityKeysValues = securityKeysValues;
            _encryptor = encryptor;
            _userRepository = userRepository;
            _dbContext = dbContext;

        }

        public async Task<EncryptorResultModel> RegisterUser(NewRegisterModel newRegister)
        {
            var encryptedPassword = _encryptor.PasswordEncrypt(newRegister.Password);

            var newUser = new User()
            {
                Id = Guid.NewGuid(),
                HashedPassword = encryptedPassword.PasswordHash,
                Salt = encryptedPassword.SaltValue,
                Email = newRegister.Email,
                FirstName = newRegister.FistName,
                Name = newRegister.Name,
                UserName = newRegister.UserName,

            };

            await _userRepository.AddAsync(newUser);
            await _dbContext.SaveChangesAsync();
            
            return encryptedPassword;
        }
    }
}
