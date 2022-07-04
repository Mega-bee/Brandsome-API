using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Brandsome.DAL.Models
{
    [Table("PostLike")]
    public partial class PostLike
    {
        [Key]
        public int Id { get; set; }
        public int? PostId { get; set; }
        [StringLength(450)]
        public string UserId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedDate { get; set; }
        public bool? IsDeleted { get; set; }
        [Column("IMEI")]
        [StringLength(31)]
        public string Imei { get; set; }

        [ForeignKey("PostId")]
        [InverseProperty("PostLikes")]
        public virtual Post Post { get; set; }
        [ForeignKey("UserId")]
        [InverseProperty("PostLikes")]
        public virtual AspNetUser User { get; set; }
    }
}
