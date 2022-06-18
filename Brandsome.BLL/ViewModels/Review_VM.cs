using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brandsome.BLL.ViewModels
{
    public partial class ReviewBase_VM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public partial class CreateReview_VM
    {
        [Required]
        public int BusinessId { get; set; }
        [Required]
        public string Description { get; set; }
    }
}
