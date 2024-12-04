namespace WeVibe.Core.Contracts.Discount
{
    public class DiscountDto
    {
        public int DiscountId { get; set; }
        public string Name { get; set; }
        public decimal Percentage { get; set; }
        public DateTime Duration { get; set; }
    }
}
