namespace zioAqua.model
{
    public class CreateUserRequest
    {
        public int UserID { get; set; }

        public int BusinessID { get; set; }

        public string LoginName { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public string UserName { get; set; } = string.Empty;

        public string? Email { get; set; }

        public string? MobileNum { get; set; }

        public string Role { get; set; } = "Staff";

        public bool IsActive { get; set; } = true;

        public DateTime CreatedDate { get; set; }
    }
}