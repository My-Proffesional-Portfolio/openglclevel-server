using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace openglclevel_server_models.Exceptions
{
    public class FriendlyException : Exception
    {
        public FriendlyException(string message) : base(message) { }
        
    }
}
