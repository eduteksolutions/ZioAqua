using System.ComponentModel.DataAnnotations;
using zioAqua.model.zioAqua.model;

namespace zioAqua.model
{
    public class AccTransactionDetail
    {
        public int TransactionDetailId { get; set; }

        public int TransactionId { get; set; }

        public int aCode { get; set; }

        public decimal Debit { get; set; }

        public decimal Credit { get; set; }

        public AccTransactionMaster? Transaction { get; set; }
    }
}
