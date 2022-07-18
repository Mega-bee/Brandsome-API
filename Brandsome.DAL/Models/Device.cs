using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Brandsome.DAL.Models
{
    [Table("Device")]
    public partial class Device
    {
        public Device()
        {
            PostViews = new HashSet<PostView>();
        }

        [Key]
        public int Id { get; set; }
        [Column("IMEI")]
        [StringLength(31)]
        public string Imei { get; set; }
        public bool? IsMobile { get; set; }
        [StringLength(511)]
        public string DeviceName { get; set; }

        [InverseProperty("Device")]
        public virtual ICollection<PostView> PostViews { get; set; }
    }
}
