using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Brandsome.DAL.Models
{
    [Table("BoostMetricLevel")]
    public partial class BoostMetricLevel
    {
        public BoostMetricLevel()
        {
            BoostMetricRewardLogs = new HashSet<BoostMetricRewardLog>();
        }

        [Key]
        public int Id { get; set; }
        public int? BoostMetricId { get; set; }
        public int? MetricLevel { get; set; }
        public int? Reward { get; set; }

        [ForeignKey("BoostMetricId")]
        [InverseProperty("BoostMetricLevels")]
        public virtual BoostMetric BoostMetric { get; set; }
        [InverseProperty("BoostMetricLevel")]
        public virtual ICollection<BoostMetricRewardLog> BoostMetricRewardLogs { get; set; }
    }
}
