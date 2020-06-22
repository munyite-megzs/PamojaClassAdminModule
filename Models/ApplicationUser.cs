
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace PamojaClassroomAdminModule.Models
{
    public class ApplicationUser : IdentityUser
    {
        [PersonalData]
        public string Name { get; set; }
        [PersonalData]
        public bool IsVerified { get; set; } 
        [NotMapped]
        public string Token { get; set; }
        public string Module { get; set; }
    }
}
