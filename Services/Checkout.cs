using Data;
using Services.Data;
using System;
using System.Collections.Generic;
using System.Linq;
namespace Services
{
    public class Checkout
    {
        IEnumerable<PriceRule> _pricingRules;
        ProductService _productService;
        IList<CheckoutItem> _items;

        public Checkout(IEnumerable<PriceRule> pricingRules)
        {
            _pricingRules = pricingRules;
            _productService = new ProductService();
            _items = new List<CheckoutItem>();
        }

        public void Clear()
        {
            _items.Clear();
        }

        public void Scan(string sku)
        {
            if (string.IsNullOrEmpty(sku))
            {
                throw new Exception("SKU cannot be NULL or Empty");
            }

            var product = _productService.GetProductBySku(sku.Trim());

            if (product==null)
            {
                throw new Exception($"Product not found for SKU : {sku}");
            }

            var checkoutItem = new CheckoutItem {
                Product = product,
                ItemNumber = _items.Count+1
            };
            _items.Add(checkoutItem);
        }

        public decimal Total()
        {
            decimal result = 0;

            // Reset Price
            foreach (var item in _items)
            {
                item.DiscountSourceItem = null;
                item.PriceRule = null;
                item.AfterDiscount = null;
            }

            // Calculate Discount
            foreach (var item in _items)
            {
                if (!item.AfterDiscount.HasValue)
                {
                    ApplyDiscount(item);
                }
            }

            foreach (var item in _items)
            {
                if (item.AfterDiscount.HasValue)
                {
                    result = result + item.AfterDiscount.Value;
                }
                else
                {
                    result = result + item.Product.Price;
                }
            }
            return result;
        }

        private void ApplyDiscount(CheckoutItem item)
        {
            var productSku = item.Product.Sku;
            var priceRule = _pricingRules.FirstOrDefault(m => m.Sku == productSku); // get pricing rule for SKU
            if (priceRule!=null) // Pricing Rule set for SKU
            {
                // Check "Rule Condition" if provided
                var condition = priceRule.Condition;
                if (condition!=null)
                {
                    if (!string.IsNullOrEmpty(condition.TargetSkuCountExpression))
                    {
                        var evalResult = ConditionEvalTargetSkuCountExpression(productSku, condition.TargetSkuCountExpression);
                        if (!evalResult)
                        {
                            return;
                        }

                    }
                    else if (condition.BuyAndGetUnitDeal.HasValue)
                    {
                        var evalResult = ConditionEvalBuyAndGetUnitDeal(productSku, condition.BuyAndGetUnitDeal.Value);
                        if (!evalResult)
                        {
                            return;
                        }
                    }
                }

                // Run "Price Rule Result"
                var ruleResult = priceRule.Result;
                if (ruleResult!=null)
                {
                    if (ruleResult.DiscountPercetage.HasValue)
                    {
                        var afterDiscount =(ruleResult.DiscountPercetage.Value/100) * item.Product.Price;
                        item.AfterDiscount = afterDiscount;
                        item.PriceRule = priceRule;
                    }
                    else if (ruleResult.Price.HasValue)
                    {
                        var afterDiscount = ruleResult.Price;
                        item.AfterDiscount = afterDiscount;
                        item.PriceRule = priceRule;
                    }
                    else if (!string.IsNullOrEmpty(ruleResult.FreeSku) && ruleResult.FreeSkuUnit.HasValue)
                    {
                        if (condition!=null 
                            && condition.BuyAndGetUnitDeal.HasValue
                            && ruleResult.FreeSku == productSku
                            && (ruleResult.FreeSkuUnit.Value + condition.BuyAndGetUnitDeal.Value) > _items.Count(m => m.Product.Sku==productSku))
                        {
                            return;
                        }

                        var freeSkuItems = _items.Where(m =>
                                                        m.Product.Sku == ruleResult.FreeSku
                                                        && m.ItemNumber>item.ItemNumber
                                                        && m.AfterDiscount == null )
                                                .Take(ruleResult.FreeSkuUnit.Value);

                        foreach (var freeItem in freeSkuItems)
                        {
                            freeItem.DiscountSourceItem = item;
                            freeItem.AfterDiscount = 0;
                        }
                        item.PriceRule = priceRule;
                    }
                }
            } // priceRule check ends
        }

        private bool ConditionEvalTargetSkuCountExpression(string productSku, string condition)
        {
            var result = false;
            var skuItemCount = _items.Count(m => m.Product.Sku == productSku);
            var conditionParts = condition.Split(' ');
            var opr = conditionParts[0];
            var valueToCompare = int.Parse(conditionParts[1]);
            result = (opr == ">" && skuItemCount > valueToCompare) ||
                     (opr == ">" && skuItemCount >= valueToCompare) ||
                     (opr == "<" && skuItemCount < valueToCompare) ||
                     (opr == "<=" && skuItemCount <= valueToCompare);
            return result;
        }

        private bool ConditionEvalBuyAndGetUnitDeal(string productSku, int buyAndGetDealCondition)
        {
            var skuItemCount = _items.Count(m => m.Product.Sku == productSku);
            return skuItemCount >= buyAndGetDealCondition;
        }
    }
}
