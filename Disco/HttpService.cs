using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Disco
{
    enum HTTPMethod
    {
        GET,
        POST
    }

    public interface IHttpService : IDisposable
    {
        Task<TReturn> GetAsync<TReturn>(string endpoint);
        void PostAsync(string endpoint, object content);
    }

    public class HttpService : IHttpService
    {
        public static string BaseUrl = "https://discordapp.com/api/";

        public HttpService()
        {

        }

        /// <summary>
        /// Performs an Async GET request
        /// </summary>
        /// <typeparam name="TReturn">Return Object</typeparam>
        /// <param name="endpoint">API Endpoint eg; /do/something</param>
        /// <returns>Request object type</returns>
        public async Task<TReturn> GetAsync<TReturn>(string endpoint)
        {
            var responseString = await Request(HTTPMethod.GET, endpoint);
            var responseObject = await Task.Run(() =>
            {
                return JsonConvert.DeserializeObject<TReturn>(responseString);
            });

            return responseObject;
        }

        /// <summary>
        /// Performs a POST request
        /// </summary>
        /// <param name="endpoint">API Endpoint eg; /do/something</param>
        /// <param name="content">Object to POST</param>
        public async void PostAsync(string endpoint, object content)
        {
            var json = await Task.Run(() =>
            {
                return JsonConvert.SerializeObject(content);
            });

            await Request(HTTPMethod.POST, endpoint, json);
        }

        // Worker class performs HTTP Requests
        private async Task<string> Request(HTTPMethod method, string endpoint, string content = null)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bot", Discord.BotToken);

                HttpResponseMessage response = null;

                switch (method)
                {
                    case HTTPMethod.GET:
                        response = await client.GetAsync(endpoint);
                        break;
                    case HTTPMethod.POST:
                        response = await client.PostAsync(endpoint, new StringContent(content, Encoding.UTF8, "application/json"));
                        break;
                }

                if (response.IsSuccessStatusCode)
                {
                    // Success
                    return await response.Content.ReadAsStringAsync();
                }
                else
                {
                    // TODO
                    return string.Empty;
                }
            }
        }

        public void Dispose()
        {
            // Nothing to dispose
        }
    }
}