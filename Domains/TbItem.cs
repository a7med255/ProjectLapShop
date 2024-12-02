using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProjectLapShop.Models;

public partial class TbItem
{
    public int ItemId { get; set; }
    [Required(ErrorMessage ="please enter item Name")]
    public string ItemName { get; set; } = null!;
    [Required(ErrorMessage = "please enter Sales Price")]
    [DataType(DataType.Currency,ErrorMessage ="please enter currency")]
    [Range(50,100000, ErrorMessage ="please enter price in system range")]
    public decimal SalesPrice { get; set; }
    [Required(ErrorMessage = "please enter Sales Price")]
    [DataType(DataType.Currency, ErrorMessage = "please enter currency")]
    [Range(50, 100000, ErrorMessage = "please enter price in system range")]
    public decimal PurchasePrice { get; set; }
    [Required(ErrorMessage = "please enter category")]
    public int CategoryId { get; set; }

    public string? ImageName { get; set; }
    public DateTime CreatedDate { get; set; }
    public string CreatedBy { get; set; } = null!;
    public int CurrentState { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public string? Description { get; set; }
    [Required(ErrorMessage = "please enter Gpu")]
    public string? Gpu { get; set; }

    public string? HardDisk { get; set; }

    public int? ItemTypeId { get; set; }

    public string? Processor { get; set; }
    [Required(ErrorMessage = "please enter Ram Size")]
    [Range(1,500, ErrorMessage = "please enter Ram in range")]
    public int? RamSize { get; set; }

    public string? ScreenReslution { get; set; }

    public string? ScreenSize { get; set; }

    public string? Weight { get; set; }
    [Required(ErrorMessage = "please enter os")]
    public int? OsId { get; set; }
    [ValidateNever]
    public virtual TbCategory Category { get; set; } = null!;

    public virtual TbItemType? ItemType { get; set; }

    public virtual TbO? Os { get; set; }

    public virtual ICollection<TbItemDiscount> TbItemDiscounts { get; set; } = new List<TbItemDiscount>();

    public virtual ICollection<TbItemImage> TbItemImages { get; set; } = new List<TbItemImage>();

    public virtual ICollection<TbPurchaseInvoiceItem> TbPurchaseInvoiceItems { get; set; } = new List<TbPurchaseInvoiceItem>();

    public virtual ICollection<TbSalesInvoiceItem> TbSalesInvoiceItems { get; set; } = new List<TbSalesInvoiceItem>();

    public virtual ICollection<TbCustomer> Customers { get; set; } = new List<TbCustomer>();
}
