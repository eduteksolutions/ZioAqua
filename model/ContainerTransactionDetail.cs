using System.ComponentModel.DataAnnotations;

namespace zioAqua.model
{
    public class ContainerTransactionDetail
    {
        [Key]
        public int DetailId { get; set; }

        public int TransactionId { get; set; }

        public int ContainerId { get; set; }

        public int Qty { get; set; }

        public decimal Rate { get; set; }

        public decimal Amount { get; set; }


        // Navigation Property
        public ContainerTransactionMaster? Transaction { get; set; }
    }

}
