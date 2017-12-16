using Microsoft.VisualStudio.TestTools.UnitTesting;
using Services;
namespace UnitTest
{
    [TestClass]
    public class AppTest
    {
        Checkout co;
        public AppTest()
        {
            var priceRuleSvc = new PriceRuleService();
            co = new Checkout(priceRuleSvc.GetPricingRules());
        }
      
        [TestMethod]
        public void Bulk_Pricing()
        {
            co.Clear();
            for (int i = 0; i < 5; i++)
            {
                co.Scan("nx9");
            }
            var total = co.Total();
            var expectValue = 2499.95m;
            Assert.IsTrue(total == expectValue, "Can calculate Bulk Pricing" );
        }

        [TestMethod]
        public void Buy_3_for_2_Deal()
        {
            co.Clear();
            for (int i = 0; i < 3; i++)
            {
                co.Scan("atv");
            }
            var total = co.Total();
            var expectValue = 219m;
            Assert.IsTrue(total == expectValue, "Can calculate Buy 3 for 2 deal");
        }


        [TestMethod]
        public void Free_Bundle()
        {
            co.Clear();
            co.Scan("mbp");
            co.Scan("hdm");
            co.Scan("hdm");

            var total = co.Total();
            var expectValue = 1429.99m;
            Assert.IsTrue(total == expectValue, "Can calculate price with Free Bundle");
        }


    }
}
