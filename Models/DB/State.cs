using System;
using System.Collections.Generic;

#nullable disable

namespace WEMA_BANK.Models.DB
{
    public partial class State
    {
        public State()
        {
            Lgas = new HashSet<Lga>();
        }

        public int StateId { get; set; }
        public string StateName { get; set; }

        public virtual ICollection<Lga> Lgas { get; set; }
    }
}
