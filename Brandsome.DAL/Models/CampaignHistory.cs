using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Brandsome.DAL.Models
{
    [Table("CampaignHistory")]
    public partial class CampaignHistory
    {
        public CampaignHistory()
        {
            CampaignPools = new HashSet<CampaignPool>();
        }

        [Key]
        public int Id { get; set; }
        public int? PostId { get; set; }
        public int? AmountInitial { get; set; }
        public int? AmountSpend { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? StartDate { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? EndDate { get; set; }

        [ForeignKey("PostId")]
        [InverseProperty("CampaignHistories")]
        public virtual Post Post { get; set; }
        [InverseProperty("Campaign")]
        public virtual ICollection<CampaignPool> CampaignPools { get; set; }
    }
}
