using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DAL.Models
{
    [Table("ClientProfile")]
    public partial class ClientProfile
    {
        [Key]
        [Column("ID")]
        public int Id { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedDate { get; set; }
        public bool? IsDeleted { get; set; }
        [StringLength(511)]
        public string Name { get; set; }
        [StringLength(511)]
        public string Email { get; set; }
        [StringLength(511)]
        public string PhoneNumber { get; set; }
        [StringLength(7)]
        public string CountryCode { get; set; }
        [Column("DOB", TypeName = "datetime")]
        public DateTime? Dob { get; set; }
        [StringLength(450)]
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        [InverseProperty("ClientProfiles")]
        public virtual AspNetUser User { get; set; }
    }
}
