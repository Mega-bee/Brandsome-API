using Brandsome.DAL.Models;
using Brandsome.DAL.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brandsome.DAL.Repos
{
    internal class BusinessFollowLogRepository : GenericRepos<BusinessFollowLog>, IBusinessFollowLogRepository
    {
        public BusinessFollowLogRepository(BrandsomeDbContext context) : base(context)
        {
        }
    }
}
