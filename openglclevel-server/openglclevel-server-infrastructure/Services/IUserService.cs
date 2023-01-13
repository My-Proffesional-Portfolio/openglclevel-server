using openglclevel_server_models.Requests.Accounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace openglclevel_server_infrastructure.Services
{
    public interface IUserService
    {
        public string RegisterUser(NewRegisterModel newRegister);
    }
}
