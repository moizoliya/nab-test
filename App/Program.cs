using Data;
using Services;
using System;
using System.Collections.Generic;

namespace App
{
    class Program
    {

        static PriceRuleService priceRuleService = new PriceRuleService();
        static IEnumerable<PriceRule> pricingRules = priceRuleService.GetPricingRules();

        static void Main(string[] args)
        {
            try
            {
                ScanString("atv, atv, atv, hdm");
                ScanString("atv, nx9, nx9, atv, nx9, nx9 , nx9");
                ScanString("mbp, hdm, nx9");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error : " + ex.Message);
            }
            Console.ReadKey();    

        }
        


        static void ScanString(string scannerString)
        {
            var co = new Checkout(pricingRules);
            foreach (var item in scannerString.Split(','))
            {
                co.Scan(item.Trim());
            }
            Console.Write($"SKUs Scanned : {scannerString} \t\t");
            Console.WriteLine("Total : $" + co.Total());
        }
    }
}
