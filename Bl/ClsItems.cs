using ProjectLapShop.Models;

namespace ProjectLapShop.Bl
{
    public interface IItems
    {
        public List<TbItem> GetAll();
        public List<VwItem> GetAllItemsData(int? categoryid);
        public List<VwItem> GetRecommendedItems(int itemId);

        public TbItem GetById(int id);
        public VwItem GetItemId(int id);
        public bool Save(TbItem item);
        public bool Delete(int id);

    }

    public class ClsItems : IItems
    {
        LapShopContext context;

        public ClsItems(LapShopContext ctx)
        {
            context = ctx; 
        }

        public List<TbItem> GetAll()
        {
            try
            {
                var items = context.TbItems.ToList();
                return items;
            }
            catch
            {

                return new List<TbItem>();
            }
        }
        public List<VwItem> GetAllItemsData(int? categoryid)
        {
            try
            {
                var items = context.VwItems.Where(a=>(a.CategoryId== categoryid || categoryid == null || categoryid==0) && a.CurrentState==1).ToList();
                return items;
            }
            catch
            {

                return new List<VwItem>();
            }
        }
        public List<VwItem> GetRecommendedItems(int itemId)
        {
            try
            {
                var item = GetById(itemId);
                var items = context.VwItems.Where(a=>a.SalesPrice>=item.SalesPrice - 150 && a.SalesPrice<item.SalesPrice+150 
                && a.CurrentState==1).OrderByDescending(a=>a.CreatedBy).ToList();
                return items;
            }
            catch
            {

                return new List<VwItem>();
            }
        }

        public TbItem GetById(int id)
        {
            try
            {
                var item = context.TbItems.FirstOrDefault(a => a.ItemId == id && a.CurrentState==1);
                return item;
            }
            catch
            {
                return new TbItem();

            }

        }
        public VwItem GetItemId(int id)
        {
            try
            {
                var item = context.VwItems.FirstOrDefault(a => a.ItemId == id && a.CurrentState == 1);
                return item;
            }
            catch
            {
                return new VwItem();

            }

        }

        public bool Save(TbItem item)
        {
            try
            {
                if (item.ItemId == 0)
                {
                    item.CurrentState = 1;
                    item.CreatedBy = "1";
                    item.CreatedDate = DateTime.Now;
                    context.TbItems.Add(item);
                }
                else
                {
                    item.UpdatedDate = DateTime.Now;
                    item.UpdatedBy = "1";
                    context.Entry(item).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                }
                context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Delete(int id)
        {
            try
            {
                var item = GetById(id);
                item.CurrentState=0;
                context.Entry(item).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                context.SaveChanges();
                return true;
            }
            catch { return false; }
        }
    }
}
