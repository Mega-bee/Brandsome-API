using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Brandsome.DAL.Models
{
    public partial class PostMedium
    {
        [Key]
        public int Id { get; set; }
        public int? PostId { get; set; }
        public int? PostTypeId { get; set; }
        [StringLength(511)]
        public string FilePath { get; set; }
        public bool? IsDeleted { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedDate { get; set; }

        [ForeignKey("PostId")]
        [InverseProperty("PostMedia")]
        public virtual Post Post { get; set; }
        [ForeignKey("PostTypeId")]
        [InverseProperty("PostMedia")]
        public virtual PostType PostType { get; set; }
    }
}
