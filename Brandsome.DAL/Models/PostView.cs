using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Brandsome.DAL.Models
{
    [Table("PostView")]
    public partial class PostView
    {
        [Key]
        public int Id { get; set; }
        public int? DeviceId { get; set; }
        public int? PostId { get; set; }
        [StringLength(450)]
        public string UserId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedDate { get; set; }

        [ForeignKey("DeviceId")]
        [InverseProperty("PostViews")]
        public virtual Device Device { get; set; }
        [ForeignKey("PostId")]
        [InverseProperty("PostViews")]
        public virtual Post Post { get; set; }
        [ForeignKey("UserId")]
        [InverseProperty("PostViews")]
        public virtual AspNetUser User { get; set; }
    }
}
