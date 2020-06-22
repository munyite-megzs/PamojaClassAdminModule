

using PamojaClassroomAdminModule.Models;
using System.Threading.Tasks;

namespace PamojaClassroomAdminModule.Repository.IRepository
{
    public interface IUserManipulation : IRepository<UserManipulation>
    {
        Task<bool> DeleteUserAsync(string url, UserManipulation objToDelete, string token);
    }
}
