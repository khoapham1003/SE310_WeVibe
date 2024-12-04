namespace WeVibe.Core.Contracts.Discount
{
    public class CreateDiscountDto
    {
        public string Name { get; set; }
        public decimal Percentage { get; set; }
        public DateTime Duration { get; set; }
    }
}
