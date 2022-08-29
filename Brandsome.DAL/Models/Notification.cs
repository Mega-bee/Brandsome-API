using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Brandsome.DAL.Models
{
    [Table("Notification")]
    public partial class Notification
    {
        [Key]
        public int Id { get; set; }
        [StringLength(450)]
        public string UserId { get; set; }
        [StringLength(450)]
        public string InitiatorId { get; set; }
        public int? EventId { get; set; }
        public int? BusinessId { get; set; }
        public int? PostId { get; set; }
        public int? ReviewId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedDate { get; set; }

        [ForeignKey("BusinessId")]
        [InverseProperty("Notifications")]
        public virtual Business Business { get; set; }
        [ForeignKey("InitiatorId")]
        [InverseProperty("NotificationInitiators")]
        public virtual AspNetUser Initiator { get; set; }
        [ForeignKey("PostId")]
        [InverseProperty("Notifications")]
        public virtual Post Post { get; set; }
        [ForeignKey("ReviewId")]
        [InverseProperty("Notifications")]
        public virtual BusinessReview Review { get; set; }
        [ForeignKey("UserId")]
        [InverseProperty("NotificationUsers")]
        public virtual AspNetUser User { get; set; }
    }
}
