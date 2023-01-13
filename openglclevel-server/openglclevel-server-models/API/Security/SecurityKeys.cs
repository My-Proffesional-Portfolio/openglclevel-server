using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace openglclevel_server_models.API.Security
{
    public class SecurityKeys : ISecurityKeys
    {
        public string passwordPrivateKey { get; set; }
        public string JWT_PrivateKey { get; set; }
        public string issuer { get; set; }
    }
}
