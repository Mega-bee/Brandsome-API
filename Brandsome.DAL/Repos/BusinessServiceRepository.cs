using Brandsome.DAL.Models;
using Brandsome.DAL.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brandsome.DAL.Repos
{
    public class BusinessServiceRepository : GenericRepos<BusinessService>, IBusinessServiceRepository
    {
        public BusinessServiceRepository(BrandsomeDbContext context) : base(context)
        {
        }
    }
}
