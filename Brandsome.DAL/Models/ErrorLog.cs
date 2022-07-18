using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Brandsome.DAL.Models
{
    [Table("ErrorLog")]
    public partial class ErrorLog
    {
        [Key]
        [Column("ID")]
        public int Id { get; set; }
        [StringLength(200)]
        public string MachineName { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime Logged { get; set; }
        [Required]
        [StringLength(5)]
        [Unicode(false)]
        public string Level { get; set; }
        [Required]
        public string Message { get; set; }
        [StringLength(300)]
        public string Logger { get; set; }
        public string Properties { get; set; }
        [StringLength(300)]
        public string Callsite { get; set; }
        public string Exception { get; set; }
    }
}
