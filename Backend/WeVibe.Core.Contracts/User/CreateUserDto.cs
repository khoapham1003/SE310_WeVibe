namespace WeVibe.Core.Contracts.User
{
    public class CreateUserDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public List<string> Roles { get; set; }
    }
}
