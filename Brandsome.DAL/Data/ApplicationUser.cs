using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Brandsome.DAL.Data
{
    public class ApplicationUser : IdentityUser
    {

        public string FcmToken { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Image { get; set; }
        public int Balance { get; set; }
        public string RecoveryEmail { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Otp { get; set; }
        public int? GenderId { get; set; }
        public bool? IsDeleted { get; set; }
        public string Name { get; set; }
        public string CountryCode { get; set; }
    }
}
