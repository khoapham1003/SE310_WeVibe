namespace WeVibe.Core.Domain.Common
{
    public class RefreshToken
    {
        public string Token { get; set; }
        public string UserId { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
}
