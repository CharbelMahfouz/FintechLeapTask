﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DAL.Models
{
    [Table("ApiDataLogging")]
    public partial class ApiDataLogging
    {
        [Key]
        public int Id { get; set; }
        [Column("profileId")]
        [StringLength(450)]
        public string ProfileId { get; set; }
        [StringLength(255)]
        public string ApiName { get; set; }
        public string JsonResponse { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedDate { get; set; }
        public string ApiParameters { get; set; }
    }
}
