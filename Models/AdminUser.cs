namespace SAFORIX.Models
{
    public class AdminUser
    {
        public int Id { get; set; }
        public required string Username { get; set; }
        public required string Password { get; set; }
    }
}
