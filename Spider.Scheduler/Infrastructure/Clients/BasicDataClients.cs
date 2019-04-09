using Marvin.StreamExtensions;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace Scheduler.API.Infrastructure.Clients
{
    public class BasicDataClients
    {
        private readonly CancellationTokenSource _cTokenSource = new CancellationTokenSource();
        private HttpClient _client;        
        public BasicDataClients(HttpClient client)
        {
            _client = client;
            _client.Timeout = new TimeSpan(0, 0, 30);
            _client.DefaultRequestHeaders.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));            
        }

        public async Task<List<string>> FetchLanguageList(string webApiUrl)
        {                        
            var request = new HttpRequestMessage(HttpMethod.Post, webApiUrl);

            using (var response = await _client.SendAsync(
                request,
              HttpCompletionOption.ResponseHeadersRead,
              _cTokenSource.Token))
            {
                var stream = await response.Content.ReadAsStreamAsync();
                response.EnsureSuccessStatusCode();
                return stream.ReadAndDeserializeFromJson<List<string>>();

            }
        }        
    }
}
