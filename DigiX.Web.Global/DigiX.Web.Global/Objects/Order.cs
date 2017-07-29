namespace DigiX.Web.Global.Objects
{
    public class Order
    {
        public Product Product { get; set; }
        public int Quantity { get; set; }
        public double TotalPrice { get; set; }
        public IDeals ProductDeals { get; set; }        
    }
}
