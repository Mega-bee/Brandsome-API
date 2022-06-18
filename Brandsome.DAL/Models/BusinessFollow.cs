using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Brandsome.DAL.Models
{
    [Table("BusinessFollow")]
    public partial class BusinessFollow
    {
        [Key]
        public int Id { get; set; }
        public int? BusinessId { get; set; }
        [StringLength(450)]
        public string UserId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedDate { get; set; }
        public bool? IsDeleted { get; set; }

        [ForeignKey("BusinessId")]
        [InverseProperty("BusinessFollows")]
        public virtual Business Business { get; set; }
        [ForeignKey("UserId")]
        [InverseProperty("BusinessFollows")]
        public virtual AspNetUser User { get; set; }
    }
}
