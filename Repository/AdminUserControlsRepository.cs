using PamojaClassroomAdminModule.Models;
using PamojaClassroomAdminModule.Repository.IRepository;
using System.Net.Http;

namespace PamojaClassroomAdminModule.Repository
{
    public class AdminUserControlsRepository : Repository<AdminControlsInput>, IAdminUserControlsRepository
    {
        private readonly IHttpClientFactory _clientFactory;

        public AdminUserControlsRepository(IHttpClientFactory clientFactory)
            :base(clientFactory)
        {
            _clientFactory = clientFactory;
        }

     
    }
}
