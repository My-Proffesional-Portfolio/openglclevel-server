using openglclevel_server_infrastructure.Services;
using openglclevel_server_models.API.Security;
using openglclevel_server_models.Requests.Accounts;
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
        public UserService(ISecurityKeys securityKeysValues)
        {
            _securityKeysValues = securityKeysValues;
        }

        public string RegisterUser(NewRegisterModel newRegister)
        {
            var aver = _securityKeysValues.JWT_PrivateKey;
            return "";
        }
    }
}
