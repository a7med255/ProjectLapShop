namespace ProjectLapShop.Models
{
    public class VwDetails
    {
        public VwDetails()
        {
            VwItem=new VwItem();
            lstItemImages= new List<TbItemImage> ();
            lstRecommendedItems= new List<VwItem> ();
        }

        public VwItem VwItem { get; set; }
        public List<TbItemImage> lstItemImages { get; set; }
        public List<VwItem> lstRecommendedItems { get; set; }

    }
}
