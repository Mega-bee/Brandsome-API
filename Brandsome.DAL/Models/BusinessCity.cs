using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Brandsome.DAL.Models
{
    [Table("BusinessCity")]
    public partial class BusinessCity
    {
        public BusinessCity()
        {
            Posts = new HashSet<Post>();
        }

        [Key]
        public int Id { get; set; }
        public int? CityId { get; set; }
        public int? BusinessId { get; set; }
        public bool? IsDeleted { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedDate { get; set; }

        [ForeignKey("BusinessId")]
        [InverseProperty("BusinessCities")]
        public virtual Business Business { get; set; }
        [ForeignKey("CityId")]
        [InverseProperty("BusinessCities")]
        public virtual City City { get; set; }
        [InverseProperty("BusinessCity")]
        public virtual ICollection<Post> Posts { get; set; }
    }
}
