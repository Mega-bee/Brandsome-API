using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Brandsome.DAL.Models
{
    [Table("BusinessView")]
    public partial class BusinessView
    {
        [Key]
        public int Id { get; set; }
        [StringLength(450)]
        public string UserId { get; set; }
        public int? BusinessId { get; set; }
        public int? DeviceId { get; set; }
        public int? CampaignId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedDate { get; set; }
        [Column("IMEI")]
        [StringLength(63)]
        public string Imei { get; set; }

        [ForeignKey("UserId")]
        [InverseProperty("BusinessViews")]
        public virtual AspNetUser User { get; set; }
    }
}
