using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using WEMA_BANK.Interface;
using WEMA_BANK.Models;

namespace WEMA_BANK.Services
{
    public class GetBanksService : IGetBanks
    {
        private readonly HttpClient _httpClient = new HttpClient();
        public static string _baseUrl = "https://wema-alatdev-apimgt.azure-api.net/alat-test/api/Shared/";

        public GetBanksService()
        {  
        }


        public string URL(string method)
        {
            var url = _baseUrl + method;
            return url;
        }


        public async Task<JObject> GetResults(string method)
        {
            string url = URL(method);
            HttpResponseMessage response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode == true)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                var resultObject = JsonConvert.DeserializeObject<JObject>(responseBody.ToString());
                return resultObject;
            }
            else
            {
                return null;
            }
        }


        public async Task<ResultObjects> GetBanks(string method)
        {
            ResultObjects result = new ResultObjects();

            var itemObject = await GetResults(method);

            if (itemObject == null)
            {
                return result;
            }
            else
            {
                var code = itemObject.SelectToken("statusCode");
                var message = itemObject.SelectToken("message");
                result.Message = message.ToString();
                result.Code = Convert.ToInt32(code);
            }
            return result;
        }

    }
}
