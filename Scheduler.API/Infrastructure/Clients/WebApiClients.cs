using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Scheduler.API.Infrastructure.Clients
{
    public class WebApiClients
    {        
        private HttpClient _client;        
        public WebApiClients(HttpClient client)
        {
            _client = client;            
            _client.DefaultRequestHeaders.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/plain"));            
        }

        public async Task CallWebApiAsync(string webApiUrl, string payload)
        {
            var body = new StringContent(payload);
            body.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var request = new HttpRequestMessage(HttpMethod.Post, webApiUrl)
            {
                Content = body,
            };

            using (var response = await _client.SendAsync(
                request,
              HttpCompletionOption.ResponseHeadersRead))
            {
                var stream = await response.Content.ReadAsStreamAsync();
                response.EnsureSuccessStatusCode();
            }
        }        
    }
}
