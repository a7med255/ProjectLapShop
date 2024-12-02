namespace ProjectLapShop.Models
{
    public class VwHome
    {
        public VwHome()
        {
            lstAllItems = new List<VwItem>();
            lstRecommenedItems = new List<VwItem>();
            lstNewItems = new List<VwItem>();
            lstFreeDelivary = new List<VwItem>();
            lstCategories = new List<TbCategory>();
        }
        public List<VwItem> lstAllItems { get; set; }
        public List<VwItem> lstRecommenedItems { get; set; }
        public List<VwItem> lstNewItems { get; set; }
        public List<VwItem> lstFreeDelivary { get; set; }
        public List<TbCategory>lstCategories { get; set; }
        public List<TbSlider> lstSliders { get; set; }

        public TbSettings Settings { get; set; }


    }
}
