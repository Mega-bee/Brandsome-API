using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Brandsome.DAL.Models
{
    public partial class BusinessService
    {
        public BusinessService()
        {
            Posts = new HashSet<Post>();
        }

        [Key]
        public int Id { get; set; }
        public int? ServiceId { get; set; }
        public int? BusinessId { get; set; }
        public bool? IsDeleted { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedDate { get; set; }

        [ForeignKey("BusinessId")]
        [InverseProperty("BusinessServices")]
        public virtual Business Business { get; set; }
        [ForeignKey("ServiceId")]
        [InverseProperty("BusinessServices")]
        public virtual Service Service { get; set; }
        [InverseProperty("BusinessService")]
        public virtual ICollection<Post> Posts { get; set; }
    }
}
