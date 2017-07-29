namespace DigiX.Web.Global.Objects
{
    public class FreeSameItemDeals : IDeals
    {
        public int DealsId { get; set; }
        public int DealsTypeId { get; set; }
        public int LeastItems { get; set; }
        public bool IsMultipliable { get; set; }
        public string Note { get; set; }
        public int TotalItemsCut { get; set; }
        public bool IsAdjusted { get; private set; }

        public void AdjustDeals(Order order, int totalEntitled)
        {
            var itemsCut = TotalItemsCut * totalEntitled;
            order.TotalPrice = order.TotalPrice - (order.Product.Price * itemsCut);

            IsAdjusted = true;
        }
    }
}
