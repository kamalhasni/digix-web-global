namespace DigiX.Web.Global.Objects
{
    public class PriceCutPerItemDeals : IDeals
    {
        public int DealsId { get; set; }
        public int DealsTypeId { get; set; }
        public int LeastItems { get; set; }
        public bool IsMultipliable { get; set; }
        public string Note { get; set; }
        public double PriceCut { get; set; }
        public bool IsAdjusted { get; private set; }

        public void AdjustDeals(Order order, int totalEntitled)
        {
            PriceCut = (PriceCut * totalEntitled) * order.Quantity;
            order.TotalPrice = order.TotalPrice - PriceCut;

            IsAdjusted = true;
        }
    }
}
