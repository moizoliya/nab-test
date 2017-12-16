using Data;
using System.Collections.Generic;
using System.Linq;
namespace Services
{
    public class ProductService
    {
        private IEnumerable<Product> _products;
        public ProductService()
        {
            _products = BuildProductList();
        }
        private IEnumerable<Product> BuildProductList()
        {
            var result = new List<Product>
            {
                new Product("nx9","Nexus 9",549.99m),
                new Product("mbp","MacBook Pro",1399.99m),
                new Product("atv","Apple TV",109.50m),
                new Product("hdm","HDMI adapter",30)
            };
            return result;
        }

        public Product GetProductBySku(string sku)
        {
            return _products.FirstOrDefault(m => m.Sku == sku);
        }
    
    }
}
