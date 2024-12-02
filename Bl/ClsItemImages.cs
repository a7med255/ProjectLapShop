using ProjectLapShop.Models;

namespace ProjectLapShop.Bl
{
    public interface IItemImages
    {
        public List<TbItemImage> GetById(int id);

    }

    public class ClsItemImages : IItemImages
    {
        LapShopContext context;

        public ClsItemImages(LapShopContext ctx)
        {
            context = ctx; 
        }

        
        public List<TbItemImage> GetById(int id)
        {
            try
            {
                var item = context.TbItemImages.Where(a => a.ItemId == id ).ToList();
                return item;
            }
            catch
            {
                return new List<TbItemImage>();

            }

        }

     
    }
}
