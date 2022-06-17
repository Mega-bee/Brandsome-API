using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Brandsome.DAL.Models
{
    [Table("City")]
    public partial class City
    {
        public City()
        {
            CampaignPools = new HashSet<CampaignPool>();
        }

        [Key]
        public int Id { get; set; }
        [StringLength(127)]
        public string Title { get; set; }
        public bool? IsDeleted { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedDate { get; set; }

        [InverseProperty("City")]
        public virtual ICollection<CampaignPool> CampaignPools { get; set; }
    }
}
