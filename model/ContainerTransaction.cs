namespace zioAqua.model
{
    public class ContainerTransaction
    {
        public int TransactionId { get; set; }
        public string? TransactionNo { get; set; }
        public DateTime TransactionDate { get; set; }
        public int ShopId { get; set; }
        public string? TransactionType { get; set; }
        public string? Remark { get; set; }
        public int BusinessId { get; set; }
        public string? LoginName { get; set; }
        public DateTime? LUserDt { get; set; }

        public List<ContainerTransactionDetail> Details { get; set; }
            = new();
    }
}
