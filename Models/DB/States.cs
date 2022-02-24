using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace WEMA_BANK.Models.DB
{
    public partial class States
    {
        public States()
        {
            Customers = new HashSet<Customers>();
            Lga = new HashSet<Lga>();
        }

        public int StateId { get; set; }
        public string StateName { get; set; }

        public virtual ICollection<Customers> Customers { get; set; }
        public virtual ICollection<Lga> Lga { get; set; }
    }
}
