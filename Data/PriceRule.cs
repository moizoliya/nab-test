namespace Data
{
    public class PriceRule
    {
        public string Sku { get; set; }
        public PriceRuleCondition Condition { get; set; }
        public PriceRuleResult Result { get; set; }
    }
}
