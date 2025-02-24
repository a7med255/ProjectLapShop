using Domains;
using ProjectLapShop.Models;
namespace ProjectLapShop.Bl
{
    public interface IPages
    {
        public List<TbPages> GetAll();
        public TbPages GetById(int id);
        public bool Save(TbPages itemType);
        public bool Dekete(int id);
    }

    public class ClsPages : IPages
    {
        LapShopContext context;
        public ClsPages(LapShopContext ctx)
        {
            context = ctx;
        }
        public List<TbPages> GetAll()
        {
            try
            {
                var lstCategories = context.TbPages.Where(a=>a.CurrentState==1).ToList();
                return lstCategories;
            }
            catch
            {
                return new List<TbPages>();
            }
        }

        public TbPages GetById(int id)
        {
            try
            {
                var itemType = context.TbPages.FirstOrDefault(a => a.PageId == id && a.CurrentState==1);
                return itemType;
            }
            catch
            {
                return new TbPages();
            }
        }

        public bool Save(TbPages itemType)
        {
            try
            {
                if (itemType.PageId == 0)
                {
                    itemType.CreatedBy = "1";
                    itemType.CreatedDate = DateTime.Now;
                    context.TbPages.Add(itemType);
                }
                else
                {
                    itemType.UpdatedBy = "1";
                    itemType.UpdatedDate = DateTime.Now;
                    context.Entry(itemType).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                }
                context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Dekete(int id)
        {
            try
            {
                var itemType = GetById(id);
                itemType.CurrentState = 0;
                context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
