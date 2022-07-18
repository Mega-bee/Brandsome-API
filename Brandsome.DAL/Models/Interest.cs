using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Brandsome.DAL.Models
{
    [Table("Interest")]
    public partial class Interest
    {
        [Key]
        public int Id { get; set; }
        [StringLength(450)]
        public string UserId { get; set; }
        public int? ServiceId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedDate { get; set; }
        public bool? IsDeleted { get; set; }

        [ForeignKey("ServiceId")]
        [InverseProperty("Interests")]
        public virtual Service Service { get; set; }
        [ForeignKey("UserId")]
        [InverseProperty("Interests")]
        public virtual AspNetUser User { get; set; }
    }
}
