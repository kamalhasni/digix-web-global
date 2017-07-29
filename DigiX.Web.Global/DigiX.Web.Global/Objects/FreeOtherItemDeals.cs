using System;
using System.Collections.Generic;
using System.Linq;

namespace DigiX.Web.Global.Objects
{
    public class FreeOtherItemDeals : IDeals
    {
        public int DealsId { get; set; }
        public int DealsTypeId { get; set; }
        public int LeastItems { get; set; }
        public bool IsMultipliable { get; set; }
        public string Note { get; set; }
        public IEnumerable<FreeProduct> FreeProducts { get; set; }
        public bool IsAdjusted { get; private set; }

        public IEnumerable<FreeProduct> AdjustDeals(Order order, int totalEntitled)
        {
            FreeProducts.All(it => { it.total = it.total * totalEntitled; return true; });

            IsAdjusted = true;
            return FreeProducts;
        }

        void IDeals.AdjustDeals(Order order, int totalEntitled)
        {
            throw new NotImplementedException();
        }
    }

    public class FreeProduct
    {
        public int ProductId { get; set; }
        public int total { get; set; }
    }
}
