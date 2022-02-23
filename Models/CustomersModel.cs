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
        public int PhoneNo { get; set; }
        public string StateName { get; set; }
        public string Lga { get; set; }
    }
}
