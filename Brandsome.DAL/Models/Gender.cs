using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Brandsome.DAL.Models
{
    [Table("Gender")]
    public partial class Gender
    {
        public Gender()
        {
            AspNetUsers = new HashSet<AspNetUser>();
        }

        [Key]
        public int Id { get; set; }
        [Column("TItle")]
        [StringLength(31)]
        public string Title { get; set; }
        public bool? IsDeleted { get; set; }

        [InverseProperty("Gender")]
        public virtual ICollection<AspNetUser> AspNetUsers { get; set; }
    }
}
