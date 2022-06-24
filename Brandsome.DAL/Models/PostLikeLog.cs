using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Brandsome.DAL.Models
{
    [Table("PostLikeLog")]
    public partial class PostLikeLog
    {
        [Key]
        public int Id { get; set; }
        public int? PostId { get; set; }
        [StringLength(450)]
        public string UserId { get; set; }
        public bool? IsLike { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedDate { get; set; }

        [ForeignKey("PostId")]
        [InverseProperty("PostLikeLogs")]
        public virtual Post Post { get; set; }
        [ForeignKey("UserId")]
        [InverseProperty("PostLikeLogs")]
        public virtual AspNetUser User { get; set; }
    }
}
