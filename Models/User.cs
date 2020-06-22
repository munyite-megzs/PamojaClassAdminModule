
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;


namespace PamojaClassroomAdminModule.Models
{
    public class User
    {
        [PersonalData]
        public string Name { get; set; }
        [PersonalData]
        public bool IsVerified { get; set; }
     
        public string Token { get; set; }
    }
}
