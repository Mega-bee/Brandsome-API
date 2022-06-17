using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Brandsome.DAL.Models
{
    [Table("BoostMetrcEntityType")]
    public partial class BoostMetrcEntityType
    {
        public BoostMetrcEntityType()
        {
            BoostMetrics = new HashSet<BoostMetric>();
        }

        [Key]
        public int Id { get; set; }
        [StringLength(63)]
        public string Title { get; set; }
        public bool? IsDeleted { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedDate { get; set; }

        [InverseProperty("BoostMetricEntityType")]
        public virtual ICollection<BoostMetric> BoostMetrics { get; set; }
    }
}
