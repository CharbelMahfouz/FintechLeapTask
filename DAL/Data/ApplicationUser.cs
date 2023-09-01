using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;

namespace DAL.Data
{
    public class ApplicationUser : IdentityUser
    {

        public string FcmToken { get; set; }
        public DateTime CreatedDate { get; set; }

        public bool? IsDeleted { get; set; }
        public string CountryCode { get; set; }
        public DateTime? DOB { get; set; }
        public string Name { get; set; }

    }
}
