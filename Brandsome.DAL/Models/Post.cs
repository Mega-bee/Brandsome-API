using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Brandsome.DAL.Models
{
    [Table("Post")]
    public partial class Post
    {
        public Post()
        {
            CampaignHistories = new HashSet<CampaignHistory>();
            PostLikes = new HashSet<PostLike>();
            PostMedia = new HashSet<PostMedium>();
            PostViews = new HashSet<PostView>();
        }

        [Key]
        public int Id { get; set; }
        public int? BusinessServiceId { get; set; }
        public int? BusinessCityId { get; set; }
        public int? PostLikeCount { get; set; }
        public int? PostViewCount { get; set; }
        public bool? IsDeleted { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedDate { get; set; }
        public string Descrption { get; set; }

        [ForeignKey("BusinessCityId")]
        [InverseProperty("Posts")]
        public virtual BusinessCity BusinessCity { get; set; }
        [ForeignKey("BusinessServiceId")]
        [InverseProperty("Posts")]
        public virtual BusinessService BusinessService { get; set; }
        [InverseProperty("Post")]
        public virtual ICollection<CampaignHistory> CampaignHistories { get; set; }
        [InverseProperty("Post")]
        public virtual ICollection<PostLike> PostLikes { get; set; }
        [InverseProperty("Post")]
        public virtual ICollection<PostMedium> PostMedia { get; set; }
        [InverseProperty("Post")]
        public virtual ICollection<PostView> PostViews { get; set; }
    }
}
