using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brandsome.BLL.ViewModels
{
    public partial class BaseInterests_VM
    {
        [JsonProperty(Order = 1)]
        public int Id { get; set; }
        [JsonProperty(Order = 2)]
        public string Name { get; set; }
        [JsonProperty(Order = 3)]
        public string Image { get; set; }
    }
    public partial class Service_VM : BaseInterests_VM
    {
        public string Category { get; set; } = "";
        public bool? IsUserInterest { get; set; } = false;
    }

    public partial class Category_VM : BaseInterests_VM
    {
        [JsonProperty(Order = 4)]
        public List<SubCategory_VM> SubCategories { set; get; }
    }

    public partial class SubCategory_VM : BaseInterests_VM
    {
        [JsonProperty(Order = 4)]
        public List<Service_VM> Services { set; get; }
    }

    public partial class ChooseInterests_VM
    {
        [Required]
        public List<ChosenInterest_VM> Interests { get; set; }
    }

    public partial class ChosenInterest_VM
    {
        public int ServiceId { get; set; }
        public bool IsSelected { get; set; }
    }
}
