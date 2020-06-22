using Newtonsoft.Json;
using PamojaClassroomAdminModule.Models;
using PamojaClassroomAdminModule.Repository.IRepository;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace PamojaClassroomAdminModule.Repository
{
    public class AccountRepository : Repository<User>, IAccountRepository
    {
        private readonly IHttpClientFactory _clientFactory;

        public AccountRepository(IHttpClientFactory clientFactory)
            : base(clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<User> LoginAsync(string url, AuthoriseUser objToCreate)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, url);

            if (objToCreate != null)
            {
                request.Content = new StringContent(JsonConvert.SerializeObject(objToCreate), Encoding.UTF8, "application/json");

            }
            else
            {
                return new User();
            }

            var client = _clientFactory.CreateClient();
            HttpResponseMessage responce = await client.SendAsync(request);
            if (responce.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var jsonString = await responce.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<User>(jsonString);
            }
            else
            {
                return new User();
            }
        }
    }
}
