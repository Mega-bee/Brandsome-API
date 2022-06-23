using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Brandsome.DAL.Models
{
    public partial class AspNetUser
    {
        public AspNetUser()
        {
            AspNetUserClaims = new HashSet<AspNetUserClaim>();
            AspNetUserLogins = new HashSet<AspNetUserLogin>();
            AspNetUserTokens = new HashSet<AspNetUserToken>();
            BusinessFollows = new HashSet<BusinessFollow>();
            BusinessPhoneClicks = new HashSet<BusinessPhoneClick>();
            BusinessReviews = new HashSet<BusinessReview>();
            BusinessViews = new HashSet<BusinessView>();
            Businesses = new HashSet<Business>();
            PostLikes = new HashSet<PostLike>();
            PostViews = new HashSet<PostView>();
            Roles = new HashSet<AspNetRole>();
        }

        [Key]
        public string Id { get; set; }
        [StringLength(256)]
        public string UserName { get; set; }
        [StringLength(256)]
        public string NormalizedUserName { get; set; }
        [StringLength(256)]
        public string Email { get; set; }
        [StringLength(256)]
        public string NormalizedEmail { get; set; }
        public bool? EmailConfirmed { get; set; }
        public string PasswordHash { get; set; }
        public string SecurityStamp { get; set; }
        public string ConcurrencyStamp { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public DateTimeOffset? LockoutEnd { get; set; }
        public bool LockoutEnabled { get; set; }
        public int AccessFailedCount { get; set; }
        [Column(TypeName = "date")]
        public DateTime? DateOfBirth { get; set; }
        [StringLength(511)]
        public string Image { get; set; }
        public int? Balance { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedDate { get; set; }
        [StringLength(127)]
        public string RecoveryEmail { get; set; }
        [StringLength(7)]
        public string Otp { get; set; }
        public int? GenderId { get; set; }
        [StringLength(255)]
        public string FcmToken { get; set; }

        [ForeignKey("GenderId")]
        [InverseProperty("AspNetUsers")]
        public virtual Gender Gender { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<AspNetUserClaim> AspNetUserClaims { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<AspNetUserLogin> AspNetUserLogins { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<AspNetUserToken> AspNetUserTokens { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<BusinessFollow> BusinessFollows { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<BusinessPhoneClick> BusinessPhoneClicks { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<BusinessReview> BusinessReviews { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<BusinessView> BusinessViews { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<Business> Businesses { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<PostLike> PostLikes { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<PostView> PostViews { get; set; }

        [ForeignKey("UserId")]
        [InverseProperty("Users")]
        public virtual ICollection<AspNetRole> Roles { get; set; }
    }
}
