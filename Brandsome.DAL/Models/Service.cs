using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Brandsome.DAL.Models
{
    public partial class Service
    {
        public Service()
        {
            BusinessServices = new HashSet<BusinessService>();
            CampaignPools = new HashSet<CampaignPool>();
            Interests = new HashSet<Interest>();
        }

        [Key]
        public int Id { get; set; }
        [StringLength(255)]
        public string Title { get; set; }
        public int? SubCategoryId { get; set; }
        public bool? IsDeleted { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedDate { get; set; }

        [ForeignKey("SubCategoryId")]
        [InverseProperty("Services")]
        public virtual SubCategory SubCategory { get; set; }
        [InverseProperty("Service")]
        public virtual ICollection<BusinessService> BusinessServices { get; set; }
        [InverseProperty("Service")]
        public virtual ICollection<CampaignPool> CampaignPools { get; set; }
        [InverseProperty("Service")]
        public virtual ICollection<Interest> Interests { get; set; }
    }
}
