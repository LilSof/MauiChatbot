using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ChatBotErazmus.Services
{
    public class ChatGService
    {
        private readonly HttpClient _httpClient;

        public ChatGService(string baseUrl, string apiKey)
        {
            _httpClient = new();
            _httpClient.BaseAddress = new Uri(baseUrl);
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");
        }

        public async Task<string> GetResponse(string query)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, _httpClient.BaseAddress)
            {
                Content = new StringContent("{\"model\": \"gpt-3.5-turbo\", \"prompt\": \"" +
                                            query +
                                            "\",\"temperature\": 0,\"max_tokens\": 100}",
                    Encoding.UTF8,
                    "application/json")
            };

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            var responseString = JsonConvert.DeserializeObject<dynamic>(responseContent);

            return responseString!.choices[0].text;
        }
    }
}
