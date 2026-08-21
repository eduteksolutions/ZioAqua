using System.ComponentModel.DataAnnotations;

namespace zioAqua.model
{
    public class AccLedgerMaster
    {
        [Key]
        public int ACode { get; set; }
        public string AccName { get; set; }
        public string Add1 { get; set; }
        public string Add2 { get; set; }
        public string GST { get; set; }
        public string Email { get; set; }
        public string PhoneNo { get; set; }
        public string OpenBalType { get; set; }
        public decimal OpenBal { get; set; }
        public int PCode { get; set; }
        public int BusinessId { get; set; }
        public string LoginName { get; set; }
    }
}
