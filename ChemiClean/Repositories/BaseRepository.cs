using ChemiClean.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChemiClean.Repositories
{
    public class BaseRepository
    {
        protected ChemiCleanContext _dbContext;
        public BaseRepository(ChemiCleanContext dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
