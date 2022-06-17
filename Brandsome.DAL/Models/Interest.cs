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
        public int? DevideId { get; set; }
        public int? ServiceId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedDate { get; set; }

        [ForeignKey("DevideId")]
        [InverseProperty("Interests")]
        public virtual Device Devide { get; set; }
        [ForeignKey("ServiceId")]
        [InverseProperty("Interests")]
        public virtual Service Service { get; set; }
    }
}
