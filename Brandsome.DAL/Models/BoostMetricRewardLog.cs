using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Brandsome.DAL.Models
{
    [Table("BoostMetricRewardLog")]
    public partial class BoostMetricRewardLog
    {
        [Key]
        public int Id { get; set; }
        public int? BoostMetricLevelId { get; set; }
        public int? BeneficiaryId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedDate { get; set; }

        [ForeignKey("BoostMetricLevelId")]
        [InverseProperty("BoostMetricRewardLogs")]
        public virtual BoostMetricLevel BoostMetricLevel { get; set; }
    }
}
