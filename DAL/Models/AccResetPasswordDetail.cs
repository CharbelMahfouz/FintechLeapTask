using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DAL.Models
{
    [Table("ACC_ResetPasswordDetails")]
    public partial class AccResetPasswordDetail
    {
        [Key]
        public int Id { get; set; }
        [StringLength(450)]
        public string UserId { get; set; }
        [Column("token")]
        public string Token { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedDate { get; set; }

        [ForeignKey("UserId")]
        [InverseProperty("AccResetPasswordDetails")]
        public virtual AspNetUser User { get; set; }
    }
}
