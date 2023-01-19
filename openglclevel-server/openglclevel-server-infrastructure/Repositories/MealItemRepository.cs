using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using openglclevel_server_data.DataAccess;
using openglclevel_server_infrastructure.Repositories.Interfaces;

namespace openglclevel_server_infrastructure.Repositories
{
    public class MealItemRepository : BaseRepository<MealItem>, IMealItemRepository
    {
        public MealItemRepository(OpenglclevelContext dbContext) : base(dbContext)
        {
        }
    }
}
