using System.ComponentModel.DataAnnotations;

namespace PamojaClassroomAdminModule.ViewModel
{
    public class CreateRoleViewModel
    {
        [Required]
        public string RoleName { get; set; }
    }
}
