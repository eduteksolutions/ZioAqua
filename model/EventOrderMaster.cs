using Microsoft.EntityFrameworkCore;

namespace zioAqua.model
{
    public class EventOrderMaster
    {
        [Key]
        public int EventOrderId { get; set; }

        public string? OrderNo { get; set; }

        public DateTime OrderDate { get; set; }
        //Ledger ID
        public int CustomerId { get; set; }

        public string? EventName { get; set; }

        public DateTime EventDate { get; set; }

        public string? EventAddress { get; set; }

        public string? ContactNo { get; set; }

        public string? Status { get; set; }

        public string? Remark { get; set; }

        public int BusinessId { get; set; }

        public int UserId { get; set; }

        public string? LoginName { get; set; }

        public DateTime? LUserDt { get; set; }


        public List<EventOrderDetail> Details { get; set; }
            = new();
    }
}
