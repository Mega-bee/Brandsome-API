using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Brandsome.DAL.Models
{
    [Table("BoostMetric")]
    public partial class BoostMetric
    {
        public BoostMetric()
        {
            BoostMetricLevels = new HashSet<BoostMetricLevel>();
        }

        [Key]
        public int Id { get; set; }
        [StringLength(255)]
        public string Title { get; set; }
        public int? BoostMetricEntityTypeId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedDate { get; set; }
        public bool? IsDeleted { get; set; }

        [ForeignKey("BoostMetricEntityTypeId")]
        [InverseProperty("BoostMetrics")]
        public virtual BoostMetrcEntityType BoostMetricEntityType { get; set; }
        [InverseProperty("BoostMetric")]
        public virtual ICollection<BoostMetricLevel> BoostMetricLevels { get; set; }
    }
}
