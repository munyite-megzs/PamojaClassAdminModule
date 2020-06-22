

using Newtonsoft.Json;
using PamojaClassroomAdminModule.Models;
using PamojaClassroomAdminModule.Repository.IRepository;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace PamojaClassroomAdminModule.Repository
{
    public class UserManipulationRepository : Repository<UserManipulation>, IUserManipulation
    {
        private readonly IHttpClientFactory _clientFactory;

        public UserManipulationRepository(IHttpClientFactory clientFactory)
            : base(clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<bool> DeleteUserAsync(string url, UserManipulation objToDelete, string token)
        {
            
                var request = new HttpRequestMessage(HttpMethod.Delete, url);

                if (objToDelete != null)
                {
                    request.Content = new StringContent(JsonConvert.SerializeObject(objToDelete), Encoding.UTF8, "application/json");

                }
                else
                {
                    return false;
                }

                var client = _clientFactory.CreateClient();
                if (token != null && token.Length != 0)
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                }
                HttpResponseMessage responce = await client.SendAsync(request);
                if (responce.StatusCode == System.Net.HttpStatusCode.NoContent)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            
        }
    }
}
