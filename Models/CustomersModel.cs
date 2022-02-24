using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WEMA_BANK.Models
{
    public class CustomersModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string PhoneNo { get; set; }
        public string StateName { get; set; }
        public string IsOnBoard { get; set; }
        public string Lga { get; set; }
        public string Otp { get; set; }
    }


    public class CustomerResult
    {
        public string Email { get; set; }
        public string PhoneNo { get; set; }
        public string StateName { get; set; }
        public string IsOnBoard { get; set; }
        public string Lga { get; set; }
    }
}
