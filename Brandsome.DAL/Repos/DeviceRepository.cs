using Brandsome.DAL.Models;
using Brandsome.DAL.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brandsome.DAL.Repos
{
    public class DeviceRepository : GenericRepos<Device>, IDeviceRepository
    {
        public DeviceRepository(BrandsomeDbContext context) : base(context)
        {
        }
    }
}
