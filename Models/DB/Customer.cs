using System;
using System.Collections.Generic;

#nullable disable

namespace WEMA_BANK.Models.DB
{
    public partial class Customer
    {
        public int CustomerId { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int PhoneNo { get; set; }
        public string Otp { get; set; }
        public bool? IsOnboard { get; set; }
        public int State { get; set; }
    }
}
