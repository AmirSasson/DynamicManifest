using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public static class Extensions
    {
        public static string ToJson<T>(this T obj)
        {
            return JsonConvert.SerializeObject(obj);
        }
        public static T Clone<T>(this T obj)
        {
            return (T)JsonConvert.DeserializeObject(ToJson(obj), obj.GetType());
        }

        public static T FromJson<T>(this string serialized)
        {
            return JsonConvert.DeserializeObject<T>(serialized);
        }

        public static async Task<HttpResponseMessage> PutAsJsonAsync<T>(this HttpClient client, string url, T obj)
        {
            using (var content = new StringContent(obj.ToJson(), Encoding.UTF8, "application/json"))
            {
#pragma warning disable CA2234 // Pass system uri objects instead of strings
                return await client.PutAsync(url, content);
#pragma warning restore CA2234 // Pass system uri objects instead of strings
            }
        }
        public static async Task<(T obj, HttpResponseMessage res)> GetAsJsonAsync<T>(this HttpClient client, string url)
        {
#pragma warning disable CA2234 // Pass system uri objects instead of strings
            var response = await client.GetAsync(url);
#pragma warning restore CA2234 // Pass system uri objects instead of strings
            if (response.IsSuccessStatusCode)
            {
                return ((await response.Content.ReadAsStringAsync()).FromJson<T>(), response);
            }
            return (default(T), response);
        }
        public static async Task<T> ReadBodyAsJsonAsync<T>(this HttpResponseMessage response)
        {
            return (await response.Content.ReadAsStringAsync()).FromJson<T>();
        }

        public static string ToClean(this object somePath)
        {
            return somePath.ToString().ToLower().Trim().Trim('/');
        }
        public static Task<TObj> AsTask<TObj>(this TObj any)
        {
            return Task.FromResult(any);
        }
    }
}
