using System.Collections.Generic;

namespace WEMA_BANK.Models
{
    public class StateAndLGA
    {
        public string StateName { get; set; }
        public List<string> Lga { get; set; }
    }


    public class StateResult
    {
        public int StateId { get; set; }
        public string StateName { get; set; }
        public List<LgaResult> Lgas { get; set; }
    }
    

    public class LgaResult
    {
        public int LgaId { get; set; }
        public string LgaName { get; set; }
    }

}
