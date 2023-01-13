using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace openglclevel_server_models.Requests.Accounts
{
    public class NewRegisterModel
    {
        public string UserName { get; set; }
        public string Password { get; set; } 
        public string Email { get; set; }   

    }
}
