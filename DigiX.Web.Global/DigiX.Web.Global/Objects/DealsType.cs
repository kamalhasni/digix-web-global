namespace DigiX.Web.Global.Objects
{
    public class DealsType
    {
        public int DealsTypeId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public enum LookupDealsType
    {
        FREE_SAME_ITEM = 1,
        PRICE_CUT_PER_ITEM = 2,
        FREE_OTHER_ITEM = 3
    }
}