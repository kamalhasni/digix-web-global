using DigiX.Web.Global.Helpers;
using DigiX.Web.Global.Objects;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DigiX.Web.Global.Services
{
    public class ProductService
    {
        private static string _rootDataDirectory = HttpContext.Current.Server.MapPath("~/bin/Data");

        private Lazy<IEnumerable<Product>> _products =
            new Lazy<IEnumerable<Product>>(() => JsonConvert.DeserializeObject<IEnumerable<Product>>(Utilities.ReadFileContent(_rootDataDirectory, "products.json")));

        private Lazy<IEnumerable<DealsType>> _dealsTypes =
            new Lazy<IEnumerable<DealsType>>(() => JsonConvert.DeserializeObject<IEnumerable<DealsType>>(Utilities.ReadFileContent(_rootDataDirectory, "lookupdealstype.json")));

        private Lazy<IEnumerable<ProductDeals>> _productDeals =
           new Lazy<IEnumerable<ProductDeals>>(() => JsonConvert.DeserializeObject<IEnumerable<ProductDeals>>(Utilities.ReadFileContent(_rootDataDirectory, "productdeals.json")));

        private Lazy<JArray> _dealsArray = new Lazy<JArray>(() => JArray.Parse(Utilities.ReadFileContent(_rootDataDirectory, "deals.json")));

        public IEnumerable<Product> GetProducts()
        {
            return _products.Value;
        }

        public Product GetProduct(int productId)
        {
            return _products.Value.FirstOrDefault(it => it.ProductId == productId);
        }

        public Product GetProduct(string sku)
        {
            return _products.Value.FirstOrDefault(it => it.Sku.ToLower() == sku.ToLower());
        }

        public IEnumerable<DealsType> GetDealsTypes()
        {
            return _dealsTypes.Value;
        }

        public DealsType GetDealsType(int dealsTypeId)
        {
            return _dealsTypes.Value.FirstOrDefault(it => it.DealsTypeId == dealsTypeId);
        }

        public IDeals GetProductDeals(int productId, int totalProducts, bool skipLeastItemChecking = false)
        {
            var productDeals = _productDeals.Value.FirstOrDefault(it => it.ProductId == productId);

            if (productDeals != null)
            {
                var dealsToken = _dealsArray.Value.FirstOrDefault(it => it["dealsId"].ToString() == productDeals.DealsId.ToString());
                if (dealsToken != null && (skipLeastItemChecking || totalProducts >= int.Parse(dealsToken["leastItems"].ToString())))
                {
                    var dealsType = (LookupDealsType)Enum.Parse(typeof(LookupDealsType), dealsToken["dealsTypeId"].ToString());
                    switch (dealsType)
                    {
                        case LookupDealsType.FREE_SAME_ITEM:
                            return JsonConvert.DeserializeObject<FreeSameItemDeals>(dealsToken.ToString());
                        case LookupDealsType.PRICE_CUT_PER_ITEM:
                            return JsonConvert.DeserializeObject<PriceCutPerItemDeals>(dealsToken.ToString());
                        case LookupDealsType.FREE_OTHER_ITEM:
                            return JsonConvert.DeserializeObject<FreeOtherItemDeals>(dealsToken.ToString());
                    }
                }
            }

            return null;
        }

        public Product AddProduct(Product product)
        {
            var latestProductId = _products.Value.OrderByDescending(it => it.ProductId).First().ProductId;
            product.ProductId = latestProductId++;

            var products = _products.Value.ToList();
            products.Add(product);

            Utilities.WriteFileContent(_rootDataDirectory, "products.json", JsonConvert.SerializeObject(products));

            return product;
        }

        public void UpdateProduct(Product product)
        {
            var products = _products.Value.ToList();
            var updatedProduct = GetProduct(product.ProductId);
            products.Remove(updatedProduct);
            products.Add(product);

            Utilities.WriteFileContent(_rootDataDirectory, "products.json", JsonConvert.SerializeObject(products));
        }
    }
}