using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace openglclevel_server_models.Responses
{
    public class UserMetricsModel
    {
        public string UserName { get; set; }
        public int EventNumbers { get; set; }
        public DateTime? lastEventRegistered { get; set; }
        public decimal GlcAverage { get; set; }
        public string Name { get; set; }
    }
}
