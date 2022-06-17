using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brandsome.BLL.ViewModels
{
    public partial class Post_VM
    {
        public int Id { get; set; }
        public string Image { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int LikeCount { get; set; }
        public bool IsLiked { get; set; }
        public List<BusinessCity_VM> Cities { get; set; }
        public string Type { get; set; }
    }
}
