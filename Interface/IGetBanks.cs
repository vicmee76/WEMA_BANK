using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WEMA_BANK.Models;

namespace WEMA_BANK.Interface
{
    public interface IGetBanks
    {
        string URL(string method);
        Task<JObject> GetResults(string method);
        Task<ResultObjects> GetBanks(string method);
        
    }
}
