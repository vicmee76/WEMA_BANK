﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WEMA_BANK.Models
{
    public class StateAndLGA
    {
        public string StateName { get; set; }
        public List<string> lga { get; set; }
    }


    public class StateResult
    {
        public int StateId { get; set; }
        public string StateName { get; set; }
        public List<LgaResult> lgas { get; set; }
    }
    

    public class LgaResult
    {
        public int LgaId { get; set; }
        public string LgaName { get; set; }
    }

}
