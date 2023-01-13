using openglclevel_server_data.DataAccess;
using openglclevel_server_infrastructure.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace openglclevel_server_infrastructure.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {

        private OpenglclevelContext _context;
        public UserRepository(OpenglclevelContext dbContext) : base(dbContext)
        { 
            _context = dbContext;
        }
    }
}
