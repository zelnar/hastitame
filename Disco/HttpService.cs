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

    public class HttpService : IDisposable
    {
        private readonly string _baseUrl = "https://discordapp.com/api/";
        private string _clientId;
        private AuthenticationHeaderValue _authHeader;

        public HttpService()
        {
            _authHeader = new AuthenticationHeaderValue("Bot", Discord.BotToken);
            _clientId = Discord.ClientId;
        }

        public async Task<T> GetAsync<T>(string action)
        {
            var responseString = await Request(HTTPMethod.GET, action);
            var responseObject = await Task.Run(() =>
            {
                return JsonConvert.DeserializeObject<T>(responseString);
            });

            return responseObject;
        }

        public async void PostAsync<T>(string action, T content)
        {
            var json = await Task.Run(() =>
            {
                return JsonConvert.SerializeObject(content);
            });

            await Request(HTTPMethod.POST, action, json);
        }

        public string GenerateAddBotUrl()
        {
            return _baseUrl + $"oauth2/authorize?client_id={_clientId}&scope=bot&permissions=0";
        }

        private async Task<string> Request(HTTPMethod method, string action, string content = null)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(_baseUrl);
                client.DefaultRequestHeaders.Authorization = _authHeader;

                HttpResponseMessage response = null;

                switch (method)
                {
                    case HTTPMethod.GET:
                        response = await client.GetAsync(action);
                        break;
                    case HTTPMethod.POST:
                        response = await client.PostAsync(action, new StringContent(content, Encoding.UTF8, "application/json"));
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