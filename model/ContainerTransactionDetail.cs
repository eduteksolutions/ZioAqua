namespace zioAqua.model
{
    public class ContainerTransactionDetail
    {
        public int DetailId { get; set; }

        public int TransactionId { get; set; }

        public int ContainerId { get; set; }

        public int Qty { get; set; }

        public decimal Rate { get; set; }

        public decimal Amount { get; set; }


        // Navigation Property
        public ContainerTransaction? Transaction { get; set; }
    }

}
