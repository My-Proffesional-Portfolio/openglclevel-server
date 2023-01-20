using openglclevel_server_data.DataAccess;
using openglclevel_server_infrastructure.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace openglclevel_server_infrastructure.Repositories
{
    public class MealEventRepository : BaseRepository<MealEvent>, IMealEventRepository
    {
        public MealEventRepository(OpenglclevelContext dbContext) : base(dbContext)
        {
        }
    }
}
