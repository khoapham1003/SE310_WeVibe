namespace WeVibe.Core.Contracts.User
{
    public class RegisterDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string RePassword { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
