using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Brandsome.DAL.Models
{
    [Table("CampaignPool")]
    public partial class CampaignPool
    {
        [Key]
        public int Id { get; set; }
        public int? CampaignId { get; set; }
        public int? ServiceId { get; set; }
        public int? Weight { get; set; }
        public int? CityId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedDate { get; set; }

        [ForeignKey("CampaignId")]
        [InverseProperty("CampaignPools")]
        public virtual CampaignHistory Campaign { get; set; }
        [ForeignKey("CityId")]
        [InverseProperty("CampaignPools")]
        public virtual City City { get; set; }
        [ForeignKey("ServiceId")]
        [InverseProperty("CampaignPools")]
        public virtual Service Service { get; set; }
    }
}
