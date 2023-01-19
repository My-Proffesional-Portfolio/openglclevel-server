﻿using openglclevel_server_data.DataAccess;
using openglclevel_server_infrastructure.Repositories.Interfaces;
using openglclevel_server_infrastructure.Services;
using openglclevel_server_models.API.Security;
using openglclevel_server_models.Requests.Accounts;
using openglclevel_server_models.Security;
using openglclevel_server_security.Decryptor;
using openglclevel_server_security.Encryptor;
using openglclevel_server_security.TokenManager;
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
        private readonly DecryptorEngine _decryptor;
        private readonly TokenHandlerEngine _tokenHandler;
        private readonly IUserRepository _userRepository;
        private readonly OpenglclevelContext _dbContext;
        public UserService(ISecurityKeys securityKeysValues, OpenglclevelContext dbContext,
            EncryptorEngine encryptor, IUserRepository userRepository, DecryptorEngine decryptor,
            TokenHandlerEngine tokenHandler)
        {
            _securityKeysValues = securityKeysValues;
            _encryptor = encryptor;
            _decryptor =  decryptor;
            _tokenHandler = tokenHandler;
            _userRepository = userRepository;
            _dbContext = dbContext;

        }

        public async Task<TokenResultModel> Login(string userName, string password)
        {
            var user = _userRepository.FindByExpresion(u => u.UserName == userName).FirstOrDefault();

            if (user == null)
                throw new Exception("User not found in system ");

            var decryptedSystemPassword = await _decryptor.Decrypt(user.HashedPassword, user.Salt);

            if (decryptedSystemPassword.PlainPassword != password)
                throw new Exception("Provided password is wrong ");

            var tokenClaims = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("userID", user.Id.ToString()),
                new KeyValuePair<string, string>("userName", user.Name)
            };

            var tokenData = _tokenHandler.GenerateToken(tokenClaims);
            return tokenData;

        }

        public async Task<EncryptorResultModel> RegisterUser(NewRegisterModel newRegister)
        {
            var preExistedUser = _userRepository.FindByExpresion(u => u.UserName == newRegister.UserName).FirstOrDefault();

            if (preExistedUser != null)
                throw new Exception("User already exist on system ");

            var encryptedPassword = await _encryptor.PasswordEncrypt(newRegister.Password);

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