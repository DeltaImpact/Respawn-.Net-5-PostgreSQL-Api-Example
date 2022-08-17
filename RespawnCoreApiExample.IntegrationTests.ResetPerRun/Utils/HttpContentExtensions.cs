using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace RespawnCoreApiExample.IntegrationTests.ResetPerRun.Utils
{
    public static class HttpContentExtensions
    {
        public static StringContent Serialize<T>(this T content)
        {
            var json = JsonConvert.SerializeObject(content);
            return new StringContent(json, Encoding.UTF8, "application/json");
        }

        public static async Task<T> DeserializeAsync<T>(this HttpContent content)
        {
            return JsonConvert.DeserializeObject<T>(await content.ReadAsStringAsync());
        }
    }
}