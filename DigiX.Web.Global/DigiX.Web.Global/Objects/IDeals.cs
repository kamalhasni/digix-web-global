namespace DigiX.Web.Global.Objects
{
    public interface IDeals
    {
        int DealsId { get; set; }
        int DealsTypeId { get; set; }
        int LeastItems { get; set; }
        bool IsMultipliable { get; set; }
        string Note { get; set; }
        bool IsAdjusted { get; }
        void AdjustDeals(Order order, int totalEntitled);
    }
}
