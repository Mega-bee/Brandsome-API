using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brandsome.BLL.ViewModels
{
    public partial class BusinessBase_VM
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public partial class FollowedBusiness_VM : BusinessBase_VM
    {

        public string Image { get; set; }
        public string Type { get; set; }
        public List<BusinessService_VM> Services { get; set; }
    }

    public partial class Business_VM :BusinessBase_VM
    {
        public List<BusinessService_VM> Services { get; set; }
        public List<BusinessCity_VM> Cities { get; set; }
        public int ViewCount { get; set; }
        public int PostCount { get; set; }
        public int ReviewCount { get; set; }
        public int FollowCount { get; set; }
        public string Image { get; set; }

    }

    public partial class BusinessInfo_VM : Business_VM
    {
        public string Description { get; set; }
        //public string Type { get; set; }
        public string PhoneNumber { get; set; }
        public List<Post_VM> Posts { get; set; }
        public List<ReviewBase_VM> Reviews { get; set; }
    }

    public partial class BusinessCity_VM : BusinessBase_VM
    {

    }

    public partial class BusinessService_VM : BusinessBase_VM
    {
    }

}
