using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Brandsome.DAL.Data
{
    public class ApplicationUser : IdentityUser
    {

        public string DeviceToken { get; set; }
        public DateTime BirthDate { get; set; }
        public int GenderId { get; set; }
    }
}
