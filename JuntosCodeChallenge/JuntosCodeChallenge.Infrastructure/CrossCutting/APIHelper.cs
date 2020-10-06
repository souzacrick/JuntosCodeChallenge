using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace JuntosCodeChallenge.Infrastructure.CrossCutting
{
    public class APIHelper
    {
        private readonly HttpClient _client = new HttpClient();

        public APIHelper()
        {
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public APIHelper(string url)
        {
            _client.BaseAddress = new Uri(url);
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<T> Get<T>(string method)
        {
            try
            {
                HttpResponseMessage responseMessage = await _client.GetAsync(method);

                if (responseMessage.IsSuccessStatusCode)
                {
                    string response = await responseMessage.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<T>(response);
                }

                return default;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<T> GetStringAsync<T>(string url)
        {
            try
            {
                string response = await _client.GetStringAsync(url);
                return JsonConvert.DeserializeObject<T>(response);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<T> Post<T>(string method, object o)
        {
            try
            {
                HttpResponseMessage responseMessage = await _client.PostAsync(method, new StringContent(JsonConvert.SerializeObject(o), Encoding.UTF8, "application/json"));

                if (responseMessage.IsSuccessStatusCode)
                {
                    string response = await responseMessage.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<T>(response);
                }

                return default;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}