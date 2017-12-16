using Data;
using System.Collections.Generic;

namespace Services
{
    public class PriceRuleService
    {
        /// <summary>
        /// This can be replaced by Database call 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<PriceRule> GetPricingRules()
        {
            var result = new List<PriceRule>();

            // 3 for 2 deal on Apple TV
            var ruleAppleTV = new PriceRule
            {
                Sku = "atv",
                Condition = new PriceRuleCondition { BuyAndGetUnitDeal = 2 },
                Result = new PriceRuleResult { FreeSku = "atv", FreeSkuUnit = 1 }
            };
            result.Add(ruleAppleTV);

            // Nexus 9 Bulk Discount if more than 4 - 499.99 each
            var ruleNexus = new PriceRule
            {
                Sku = "nx9",
                Condition = new PriceRuleCondition { TargetSkuCountExpression = "> 4" },
                Result = new PriceRuleResult { Price = 499.99m }
            };
            result.Add(ruleNexus);

            // HDMI adapter bundled free with every MacBook pro
            var ruleMacbookPro = new PriceRule
            {
                Sku = "mbp",
                Result = new PriceRuleResult { FreeSku = "hdm", FreeSkuUnit = 1 }
            };
            result.Add(ruleMacbookPro);
            return result;
        }
    }
}
