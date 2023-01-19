using openglclevel_server_models.Requests.Accounts;
using openglclevel_server_models.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace openglclevel_server_infrastructure.Services
{
    public interface IUserService
    {
        public  Task<EncryptorResultModel> RegisterUser(NewRegisterModel newRegister);
        public Task<TokenResultModel> Login(string userName, string password);

    }
}
