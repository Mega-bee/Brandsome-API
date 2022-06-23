using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brandsome.BLL.ViewModels
{
    public partial class SplashScreen_VM
    {
        public List<Category_VM> Categories { get; set; }
        public List<City_VM> Cities { get; set; }
    }

    public partial class City_VM
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
