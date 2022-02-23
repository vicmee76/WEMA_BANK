using System;
using System.Collections.Generic;

#nullable disable

namespace WEMA_BANK.Models.DB
{
    public partial class Lga
    {
        public int LgaId { get; set; }
        public int StateId { get; set; }
        public string LgaName { get; set; }

        public virtual State State { get; set; }
    }
}
