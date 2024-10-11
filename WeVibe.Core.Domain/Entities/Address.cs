namespace WeVibe.Core.Domain.Entities
{
    public class Address
    {
        public int AddressId { get; set; }
        public string HouseNumber { get; set; }
        public string Street { get; set; }
        public string District { get; set; }
        public string City { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}
