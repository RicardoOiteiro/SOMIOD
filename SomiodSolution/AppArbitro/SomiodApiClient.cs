using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AppArbitro
{

    public class ApiResult
    {
        public bool Ok { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public string Body { get; set; }
    }

    public class SomiodApiClient
    {
        private readonly HttpClient _http;

        public SomiodApiClient(string baseUrl)
        {
            _http = new HttpClient();
            _http.BaseAddress = new Uri(baseUrl);
        }

        public async Task<ApiResult> CreateApplicationAsync(string appName)
        {
            // JSON com o nome esperado pelo teu model: "resource-name"
            var json = $"{{\"resource-name\":\"{appName}\"}}";
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var resp = await _http.PostAsync("api/somiod", content);
            var body = await resp.Content.ReadAsStringAsync();

            return new ApiResult
            {
                Ok = resp.IsSuccessStatusCode,
                StatusCode = resp.StatusCode,
                Body = body
            };
        }

        public async Task<ApiResult> CreateContainerAsync(string appName, string contName)
        {
            var json = $"{{\"resource-name\":\"{contName}\"}}";
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // no teu controller: POST api/somiod/{appName}/containers
            var resp = await _http.PostAsync($"api/somiod/{appName}/containers", content);
            var body = await resp.Content.ReadAsStringAsync();

            return new ApiResult
            {
                Ok = resp.IsSuccessStatusCode,
                StatusCode = resp.StatusCode,
                Body = body
            };
        }

        public async Task<(bool ok, HttpStatusCode code, string body)> CreateContentInstanceAsync(
            string appName,
            string contName,
            string ciName,
            string contentType,
            string content)
        {
            var json =
                $@"{{
                    ""resource-name"": ""{ciName}"",
                    ""content-type"": ""{contentType}"",
                    ""content"": ""{content}""
                }}";

            var resp = await _http.PostAsync(
                $"api/somiod/{appName}/{contName}/contents",
                new StringContent(json, Encoding.UTF8, "application/json"));

            var bodyResp = await resp.Content.ReadAsStringAsync();
            return (resp.IsSuccessStatusCode, resp.StatusCode, bodyResp);
        }
    }
}
