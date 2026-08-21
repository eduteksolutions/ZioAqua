using System.ComponentModel.DataAnnotations;

namespace zioAqua.model
{
    public class BusinessMaster
    {
        [Key]
        public int BusinessId { get; set; }

        public string BusinessName { get; set; }

        public string? Address { get; set; }

        public string? PhoneNo { get; set; }

        public string? Email { get; set; }

        public string? GSTNo { get; set; }

        public DateTime? LUserDt { get; set; }

        public bool IsActive { get; set; }
    }
}
