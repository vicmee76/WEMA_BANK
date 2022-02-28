using System.Collections.Generic;

namespace WEMA_BANK.Models
{
    public class ResultObjects
    {
        public int Code { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
    }

    public class BanksModels
    {
        public List<BankDetails> result { get; set; }
        public string errorMessage { get; set; }
        public string errorMessages { get; set; }
        public bool hasError { get; set; }
        public string timeGenerated { get; set; }
        public int statusCode { get; set; }
        public string message { get; set; }
    }


    public class BankDetails
    {
        public string bankName { get; set; }
        public string bankCode { get; set; }
    } 

    
}
