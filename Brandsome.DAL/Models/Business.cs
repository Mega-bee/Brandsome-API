using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Brandsome.DAL.Models
{
    [Table("Business")]
    public partial class Business
    {
        public Business()
        {
            BusinessFollows = new HashSet<BusinessFollow>();
            BusinessPhoneClicks = new HashSet<BusinessPhoneClick>();
            BusinessReviews = new HashSet<BusinessReview>();
            BusinessServices = new HashSet<BusinessService>();
        }

        [Key]
        public int Id { get; set; }
        [StringLength(450)]
        public string UserId { get; set; }
        [StringLength(511)]
        public string Image { get; set; }
        public string BusinessPhone { get; set; }
        [StringLength(127)]
        public string BusinessName { get; set; }
        public string Description { get; set; }
        public int? BusinessViewCount { get; set; }
        public int? BusinessReviewCount { get; set; }
        public int? BusinessFollowCount { get; set; }
        public int? BusinessLikeCount { get; set; }
        public int? BusinessPhoneClickCount { get; set; }
        public int? BusinessPostCount { get; set; }

        [ForeignKey("UserId")]
        [InverseProperty("Businesses")]
        public virtual AspNetUser User { get; set; }
        [InverseProperty("Business")]
        public virtual ICollection<BusinessFollow> BusinessFollows { get; set; }
        [InverseProperty("Business")]
        public virtual ICollection<BusinessPhoneClick> BusinessPhoneClicks { get; set; }
        [InverseProperty("Business")]
        public virtual ICollection<BusinessReview> BusinessReviews { get; set; }
        [InverseProperty("Business")]
        public virtual ICollection<BusinessService> BusinessServices { get; set; }
    }
}
