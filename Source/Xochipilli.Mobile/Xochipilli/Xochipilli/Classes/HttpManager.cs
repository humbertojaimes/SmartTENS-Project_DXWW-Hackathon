using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Xochipilli.Classes
{
    public class HttpManager
    {
        public async static Task<string> PerformGetRequestAsync(string method, string parameters)
        {
            string result = string.Empty;
            string page = string.Format("https://xochipilli-api.azurewebsites.net/api/{0}?{1}", method, parameters);

            using (HttpClient client = new HttpClient())
            using (HttpResponseMessage response = await client.GetAsync(page))
            using (HttpContent content = response.Content)
            {
                if (response.IsSuccessStatusCode)
                    result = await content.ReadAsStringAsync();
            }

            return result;
        }

        public async static Task<string> PerformGetPredictorRequestAsync(string parameters)
        {
            string result = string.Empty;
            string page = string.Format("https://xochipillipredictor.azurewebsites.net/api/predictor/{0}", parameters);

            using (HttpClient client = new HttpClient())
            using (HttpResponseMessage response = await client.GetAsync(page))
            using (HttpContent content = response.Content)
            {
                if (response.IsSuccessStatusCode)
                    result = await content.ReadAsStringAsync();
            }

            return result;
        }

        public async static Task<string> PerformPostRequestAsync(string method, object entity)
        {
            string result = string.Empty;

            string page = string.Format("https://xochipilli-api.azurewebsites.net/api/{0}", method);

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // Use the JSON formatter to create the content of the request body.
                var json = JsonConvert.SerializeObject(entity);
                var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

                using (HttpResponseMessage response = await client.PostAsync(page, httpContent))
                using (HttpContent content = response.Content)
                {
                    if (response.IsSuccessStatusCode)
                        result = await content.ReadAsStringAsync();
                }
            }

            return result;
        }
    }
}