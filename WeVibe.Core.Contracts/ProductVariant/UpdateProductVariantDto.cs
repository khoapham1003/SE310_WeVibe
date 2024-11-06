namespace WeVibe.Core.Contracts.ProductVariant
{
    public class UpdateProductVariantDto
    {
        public string SizeName { get; set; }
        public string ColorName { get; set; }
        public string ColorHex { get; set; }
        public int Quantity { get; set; }
    }
}
