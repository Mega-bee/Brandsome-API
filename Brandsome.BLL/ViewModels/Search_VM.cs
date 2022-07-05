using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brandsome.BLL.ViewModels
{
    public partial class Search_VM
    {
        public List<FollowedBusiness_VM> Businesses { get; set; }
        public List<Service_VM> Services { get; set; }
    }
}
