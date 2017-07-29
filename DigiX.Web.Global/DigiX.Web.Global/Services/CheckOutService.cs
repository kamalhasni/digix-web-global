using DigiX.Web.Global.Objects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DigiX.Web.Global.Services
{
    public class CheckOutService
    {
        private Lazy<ProductService> _productService = new Lazy<ProductService>();
        private List<Order> _orders { get; set; }

        public CheckOutService()
        {
            _orders = new List<Order>();
        }

        public IEnumerable<Order> GetCheckoutOrders(List<string> itemSkus)
        {
            foreach (var itemSku in itemSkus)
            {
                if (!string.IsNullOrWhiteSpace(itemSku))
                {
                    var product = _productService.Value.GetProduct(itemSku.Trim());
                    if (product != null)
                    {
                        var order = _orders.FirstOrDefault(it => it.Product.ProductId == product.ProductId);
                        if (order != null)
                        {
                            order.Quantity++;
                            order.TotalPrice += product.Price;
                        }
                        else
                        {
                            _orders.Add(new Order { Product = product, Quantity = 1, TotalPrice = product.Price });
                        }
                    }
                }
            }

            if (_orders.Any())
            {
                _orders.All(it => { it.ProductDeals = _productService.Value.GetProductDeals(it.Product.ProductId, it.Quantity); return true; });

                AdjustDeals();
                return _orders.OrderBy(it => it.Product.ProductId);
            }

            return null;
        }

        private void AdjustDeals()
        {
            var freeProducts = new List<FreeProduct>();
            foreach (var order in _orders)
            {
                if (order.ProductDeals == null || order.Quantity < order.ProductDeals.LeastItems || order.ProductDeals.IsAdjusted)
                    continue;

                var totalEntitled = 1;
                if (order.ProductDeals.IsMultipliable)
                    totalEntitled = (int)Math.Floor((decimal)(order.Quantity / order.ProductDeals.LeastItems));

                var dealsType = (LookupDealsType)order.ProductDeals.DealsTypeId;
                switch (dealsType)
                {
                    case LookupDealsType.FREE_SAME_ITEM:
                    case LookupDealsType.PRICE_CUT_PER_ITEM:
                        {
                            order.ProductDeals.AdjustDeals(order, totalEntitled);
                            break;
                        }
                    case LookupDealsType.FREE_OTHER_ITEM:
                        {
                            freeProducts.AddRange(((FreeOtherItemDeals)(order.ProductDeals)).AdjustDeals(order, totalEntitled));
                            break;
                        }
                }
            }

            if (freeProducts.Any())
            {
                foreach(var freeProduct in freeProducts)
                {
                    var order = _orders.FirstOrDefault(it => it.Product.ProductId == freeProduct.ProductId);
                    if (order != null)
                    {
                        order.Quantity++;
                    }
                    else
                    {
                        _orders.Add(new Order
                        {
                            Product = _productService.Value.GetProduct(freeProduct.ProductId),
                            Quantity = freeProduct.total,
                            TotalPrice = 0.00
                        });
                    }
                }
            }
        }
    }
}
