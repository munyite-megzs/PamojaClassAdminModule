﻿using System;
using System.ComponentModel.DataAnnotations;

namespace PamojaClassroomAdminModule.Models
{
    public class GradesTaught
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public DateTime CurrentDate { get; set; }
    }
}
