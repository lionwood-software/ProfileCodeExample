using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Profile.Infrastructure.Extensions
{
    public static class HttpClientExtensions
    {
        public static async Task<(HttpResponseMessage message, TResult content)> TryDeserializeContent<TResult>(this Task<HttpResponseMessage> task)
        {
            var responseMessage = await task;
            if (!responseMessage.IsSuccessStatusCode)
            {
                return (responseMessage, default);
            }
            if (responseMessage.StatusCode == HttpStatusCode.NoContent)
            {
                return (responseMessage, default);
            }
            var content = await responseMessage.Content.ReadAsStringAsync();
            return (responseMessage, JsonConvert.DeserializeObject<TResult>(content));
        }

        public static Task<HttpResponseMessage> PostAsync<T>(this HttpClient client, string uri, T body)
        {
            var stringContent = new StringContent(
                JsonConvert.SerializeObject(body, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }),
                Encoding.UTF8,
                MediaTypeNames.Application.Json);
            return client.PostAsync(uri, stringContent);
        }

        public static Task<HttpResponseMessage> PutAsync<T>(this HttpClient client, string uri, T body)
        {
            var stringContent = new StringContent(
                JsonConvert.SerializeObject(body, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }),
                Encoding.UTF8,
                MediaTypeNames.Application.Json);
            return client.PutAsync(uri, stringContent);
        }
    }
}