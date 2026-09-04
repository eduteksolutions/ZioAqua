using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace zioAqua.model
{
    [Table("UserMaster")]
    public class UserMaster
    {
        [Key]
        public int UserID { get; set; }

        public int BusinessID { get; set; }

        [Required]
        [MaxLength(50)]
        public string LoginName { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string UserName { get; set; } = string.Empty;

        [MaxLength(150)]
        public string? Email { get; set; }

        [MaxLength(15)]
        public string? MobileNum { get; set; }

        [Required]
        [MaxLength(30)]
        public string Role { get; set; } = "Staff";

        public bool IsActive { get; set; } = true;

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    }
}