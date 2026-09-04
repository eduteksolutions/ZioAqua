namespace zioAqua.model
{
    using System.ComponentModel.DataAnnotations;

    namespace zioAqua.model
    {
        public class AccTransactionMaster
        {
            [Key]
            public int TransactionId { get; set; }

            public string? TransactionNo { get; set; }

            public DateTime TransactionDate { get; set; }

            public string? TransactionType { get; set; }

            public int BusinessId { get; set; }

            public string? Remark { get; set; }

            public string? LoginName { get; set; }

            public DateTime? LUserDt { get; set; }

            public List<AccTransactionDetail> Details { get; set; }
                = new();
        }
    }
}
