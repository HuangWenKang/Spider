using Marvin.StreamExtensions;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using VideoSpider.Models;

namespace VideoSpider.Services
{
    public class Channel9VideoSpiderService : IVideoSpiderService
    {
        private static HttpClient _httpClient = new HttpClient(
            new HttpClientHandler()
            {
                AutomaticDecompression = System.Net.DecompressionMethods.GZip
            });

        public Channel9VideoSpiderService()
        {
            _httpClient.BaseAddress = new Uri("https://search.channel9.msdn.com");
            _httpClient.Timeout = new TimeSpan(0, 0, 30);
            _httpClient.DefaultRequestHeaders.Clear();
        }

        public async Task<VideoSpiderResult> SearchAsync(string keyword, int pageNumber, int pageSize)
        {
            VideoSpiderResult result;
            var request = new HttpRequestMessage(
                HttpMethod.Get,
                $"api/v1/documents?text={keyword}&pageSize={pageSize}&pageNumber={pageNumber}&languages=en");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));

            using (var response = await _httpClient.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();

                var stream = await response.Content.ReadAsStreamAsync();
                result = stream.ReadAndDeserializeFromJson<VideoSpiderResult>();
            }
            return result;
        }
    }
}
