using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace openglclevel_server_models.Security
{
    public class DecryptorResultModel
    {
        public string PlainPassword { get; set; }
        public bool IsError { get; set; }
        public string ErrorMessage { get; set; }
        public string TechnicalMessage { get; set; }
    }
}
