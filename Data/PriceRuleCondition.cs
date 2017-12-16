namespace Data
{
    public class PriceRuleCondition
    {
        /// <summary>
        /// This is used when you have offer like Buy 2 and Get One
        /// The value for this propery in case of Buy 2 and Get one offer will be "2"
        /// </summary>
        public int? BuyAndGetUnitDeal { get; set; }


        /// <summary>
        /// This property is useful to evaluate total number of Items for contexted Sku
        /// For example :
        /// 1. total sku's item count is more than 4  : ">4"  
        /// 2. total sku's item count is less than 2 : "<2"  
        /// </summary>
        public string TargetSkuCountExpression { get; set; }

    }
}
