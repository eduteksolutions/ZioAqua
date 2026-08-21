using System.ComponentModel.DataAnnotations;

namespace zioAqua.model
{
    public class EventOrderDetail
    {
        [Key]
        public int DetailId { get; set; }
        public int EventOrderId { get; set; }
        public int ContainerId { get; set; }
        public int Qty { get; set; }
        public decimal Rate { get; set; }
        public decimal Amount { get; set; }
    }

}
