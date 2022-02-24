// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace WEMA_BANK.Models.DB
{
    public partial class Lga
    {
        public int LgaId { get; set; }
        public int StateId { get; set; }
        public string LgaName { get; set; }

        public virtual States State { get; set; }
    }
}
