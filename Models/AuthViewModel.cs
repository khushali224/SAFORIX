namespace AuthMVC.Models
{
    public class AuthViewModel
    {
        // Login
        public string LoginUsername { get; set; }
        public string LoginPassword { get; set; }

        // Register
        public string RegisterUsername { get; set; }
        public string RegisterEmail { get; set; }
        public string RegisterPassword { get; set; }
    }
}
