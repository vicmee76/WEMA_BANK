using Microsoft.Extensions.Configuration;
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
        private readonly IConfiguration _configuration;
        public static string _baseUrl = "https://wema-alatdev-apimgt.azure-api.net/alat-test/api/Shared/";

        public GetBanksService(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        public string URL(string method)
        {
            var url = _baseUrl + method;
            return url;
        }


        public async Task<BanksModels> GetResults(string method)
        {
            BanksModels result = new BanksModels();

            string url = URL(method);
            string keys = _configuration.GetSection("WemaKey").GetSection("sub-key").Value.ToString();
            _httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Ocp-Apim-Subscription-Key", keys.ToString());
            HttpResponseMessage response = await _httpClient.GetAsync(url);
            string responseBody = await response.Content.ReadAsStringAsync();
            var resultObject = JsonConvert.DeserializeObject<BanksModels>(responseBody.ToString());
            return resultObject;
        }


        

    }
}
