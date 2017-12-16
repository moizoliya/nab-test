using Data;
namespace Services.Data
{
    internal class CheckoutItem
    {
        public Product Product { get; set; }
        public PriceRule PriceRule { get; set; }
        public decimal? AfterDiscount { get; set; }
        public CheckoutItem DiscountSourceItem { get; set; }
        public int ItemNumber { get; set; }
    }
}
