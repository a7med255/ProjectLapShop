using ProjectLapShop.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace ProjectLapShop.Bl
{
    public interface ISalesInvoice
    {
        public List<VwSalesInvoice> GetAll();

        public TbSalesInvoice GetById(int id);
        public List<TbSalesInvoice> GetOrdersByCustomerId(Guid customerId);
        public bool Save(TbSalesInvoice Item, List<TbSalesInvoiceItem> lstItems, bool isNew);

        public bool Delete(int id);
    }
    public class ClsSalesInvoice : ISalesInvoice
    {
        LapShopContext ctx;
        ISalesInvoiceItems salesInvoiceItemsService;
        UserManager<ApplicationUser> _userManager;
        public ClsSalesInvoice(LapShopContext context,
            ISalesInvoiceItems invoiceItems , UserManager<ApplicationUser> userManager)
        {
            ctx = context;
            salesInvoiceItemsService = invoiceItems;
            _userManager = userManager;
        }
        public List<VwSalesInvoice> GetAll()
        {
            try
            {
                return ctx.VwSalesInvoices.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception();
            }
        }

        public TbSalesInvoice GetById(int id)
        {
            try
            {
                var Item = ctx.TbSalesInvoices.Include(a => a.TbSalesInvoiceItems).ThenInclude(i => i.Item).FirstOrDefault(a => a.InvoiceId == id);
                if (Item == null)
                    return new TbSalesInvoice();
                else
                    return Item;
            }
            catch (Exception ex)
            {
                throw new Exception();
            }
        }
        public List<TbSalesInvoice> GetOrdersByCustomerId(Guid customerId)
        {
            return ctx.TbSalesInvoices
                .Where(invoice => invoice.CustomerId == customerId)
                .Include(invoice => invoice.TbSalesInvoiceItems) // Include items if needed
                .ToList();
        }
        public bool Save(TbSalesInvoice Item,List<TbSalesInvoiceItem> lstItems, bool isNew)
        {
            using var transaction = ctx.Database.BeginTransaction();
            try
            {
                Item.CurrentState = 1;
                if (isNew)
                {
                    Item.CreatedDate = DateTime.Now;
                    ctx.TbSalesInvoices.Add(Item);
                }

                else
                {
                    Item.UpdatedBy = "1";
                    Item.UpdatedDate = DateTime.Now;
                    ctx.Entry(Item).State = EntityState.Modified;
                }

                ctx.SaveChanges();
                salesInvoiceItemsService.Save(lstItems, Item.InvoiceId, true);

                transaction.Commit();
                return true;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new Exception();
            }
        }

        public bool Delete(int id)
        {
            try
            {
                var Item = ctx.TbSalesInvoices.Where(a => a.InvoiceId == id).FirstOrDefault();
                if (Item != null)
                {
                    ctx.TbSalesInvoices.Remove(Item);
                    ctx.SaveChanges();
                    return true;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                throw new Exception();
            }
        }
    }
}
