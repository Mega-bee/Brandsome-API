using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Brandsome.DAL.Models
{
    [Table("BusinessFollowLog")]
    public partial class BusinessFollowLog
    {
        [Key]
        public int Id { get; set; }
        public int? BusinessId { get; set; }
        [StringLength(450)]
        public string UserId { get; set; }
        public bool? IsFollow { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedDate { get; set; }
        [Column("IMEI")]
        [StringLength(31)]
        public string Imei { get; set; }

        [ForeignKey("BusinessId")]
        [InverseProperty("BusinessFollowLogs")]
        public virtual Business Business { get; set; }
        [ForeignKey("UserId")]
        [InverseProperty("BusinessFollowLogs")]
        public virtual AspNetUser User { get; set; }
    }
}
