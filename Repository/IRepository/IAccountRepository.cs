using PamojaClassroomAdminModule.Models;
using System.Threading.Tasks;

namespace PamojaClassroomAdminModule.Repository.IRepository
{
    public interface IAccountRepository :IRepository<User>
    {
        Task<User> LoginAsync(string url, AuthoriseUser objToCreate);
      
    }
}
