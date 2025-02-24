using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectLapShop.Models;
using Microsoft.EntityFrameworkCore;

namespace ProjectLapShop.Bl
{
    public interface ISalesInvoiceItems
    {
        public List<TbSalesInvoiceItem> GetSalesInvoiceId(int id);

        public bool Save(IList<TbSalesInvoiceItem> Items, int salesInvoiceId, bool isNew);
    }

    public class ClsSalesInvoiceItems : ISalesInvoiceItems
    {
        LapShopContext ctx;
        public ClsSalesInvoiceItems(LapShopContext context)
        {
            ctx = context;
        }

        public List<TbSalesInvoiceItem> GetSalesInvoiceId(int id)
        {
            try
            {
                var Items = ctx.TbSalesInvoiceItems.Where(a => a.InvoiceId == id).ToList();
                if (Items == null)
                    return new List<TbSalesInvoiceItem>();
                else
                    return Items;
            }
            catch (Exception ex)
            {
                throw new Exception();
            }
        }

        public bool Save(IList<TbSalesInvoiceItem> Items,int salesInvoiceId, bool isNew)
        {
            List<TbSalesInvoiceItem> dbInvoiceItems =
                GetSalesInvoiceId(Items[0].InvoiceId);

            foreach (var interfaceItems in Items)
            {
                var dbObject = dbInvoiceItems.Where(a => a.InvoiceItemId == interfaceItems.InvoiceItemId).FirstOrDefault();
                if (dbObject != null)
                {
                    ctx.Entry(dbObject).State = EntityState.Modified;
                }
                else
                {
                    interfaceItems.InvoiceId = salesInvoiceId;
                    ctx.TbSalesInvoiceItems.Add(interfaceItems);
                }
            }

            foreach (var item in dbInvoiceItems)
            {
                var interfaceObject = Items.Where(a => a.InvoiceItemId == item.InvoiceItemId).FirstOrDefault();
                if (interfaceObject == null)
                    ctx.TbSalesInvoiceItems.Remove(item);
            }

            ctx.SaveChanges();
            return true;
        }
    }
}
