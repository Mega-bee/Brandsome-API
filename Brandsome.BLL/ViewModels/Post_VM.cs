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
        //public string Image { get; set; }
        public string Name { get; set; }
        public List<PostMedia_VM> PostMedia { get; set; }
        public string Description { get; set; }
        public int LikeCount { get; set; }
        public bool IsLiked { get; set; }
        public string City { get; set; }
        public string Type { get; set; }
    }

    public partial class PostMedia_VM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? MediaTypeId { get; set; }
        public string MediaTypeName { get; set; }
    }
}
