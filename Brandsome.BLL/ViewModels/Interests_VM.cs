using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brandsome.BLL.ViewModels
{
    public partial class BaseInterests_VM
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public partial class Service_VM : BaseInterests_VM
    {

    }

    public partial class Category_VM : BaseInterests_VM
    {

    }

    public partial class SubCategory_VM : BaseInterests_VM
    {

    }
}
