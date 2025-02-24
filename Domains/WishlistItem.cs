using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectLapShop.Models
{
    public class WishlistItem
    {
        public int Id { get; set; }
        public string UserId { get; set; } 
        public int ProductId { get; set; } 
        public DateTime DateAdded { get; set; }
    }
}
