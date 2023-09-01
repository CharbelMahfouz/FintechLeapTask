using DAL.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;

namespace BLL.ViewModels
{


    public partial class EmailSignUp_VM
    {

        [Required]
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string CountryCode { get; set; }
        [Required]
        public string FullName { get; set; }
        [Required]
        public string Password { get; set; }
        

    }

   


    public partial class EmailSignIn_VM
    {

        public string Email { get; set; }
        public string Password { get; set; }
    }

    public partial class Profile_VM
    {
        public int Id { get; set; }
        public string Token { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string CountryCode { get; set; }
        public string Role { get; set; }


    }

    public partial class UpdateProfile_VM : Profile_VM
    {
       
    }

    public partial class IdentityResult_VM
    {
        public ApplicationUser User { get; set; }
        public IdentityResult Result { get; set; }
    }
}