using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Brandsome.DAL.Models
{
    [Keyless]
    [Table("BusinessCity")]
    public partial class BusinessCity
    {
        public int? Id { get; set; }
        public int? CityId { get; set; }
        public int? BusinessId { get; set; }
        public bool? IsDeleted { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedDate { get; set; }

        [ForeignKey("BusinessId")]
        public virtual Business Business { get; set; }
        [ForeignKey("CityId")]
        public virtual City City { get; set; }
    }
}
