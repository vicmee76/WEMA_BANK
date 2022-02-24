using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace WEMA_BANK.Models.DB
{
    public partial class Customers
    {
        public int CustomerId { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PhoneNo { get; set; }
        public string Otp { get; set; }
        public bool? IsOnboard { get; set; }
        public int Location { get; set; }

        public virtual States LocationNavigation { get; set; }
    }
}
