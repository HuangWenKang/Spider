using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace Spider.Scheduler.Services
{
    public class WebApiClients
    {
        private readonly CancellationTokenSource _cTokenSource = new CancellationTokenSource();
        private HttpClient _client;
        private IBasicDataService _basicDataService;
        public WebApiClients(HttpClient client, IBasicDataService basicDataService)
        {
            _client = client;
            _client.Timeout = new TimeSpan(0, 0, 30);
            _client.DefaultRequestHeaders.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/plain"));


            _basicDataService = basicDataService;
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
              HttpCompletionOption.ResponseHeadersRead,
              _cTokenSource.Token))
            {
                var stream = await response.Content.ReadAsStreamAsync();
                response.EnsureSuccessStatusCode();
            }
        }
    }
}
