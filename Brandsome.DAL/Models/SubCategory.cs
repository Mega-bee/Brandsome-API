using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Brandsome.DAL.Models
{
    [Table("SubCategory")]
    public partial class SubCategory
    {
        public SubCategory()
        {
            Services = new HashSet<Service>();
        }

        [Key]
        public int Id { get; set; }
        [StringLength(255)]
        public string Title { get; set; }
        public int? CategoryId { get; set; }
        public bool? IsDeleted { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedDate { get; set; }

        [ForeignKey("CategoryId")]
        [InverseProperty("SubCategories")]
        public virtual Category Category { get; set; }
        [InverseProperty("SubCategory")]
        public virtual ICollection<Service> Services { get; set; }
    }
}
