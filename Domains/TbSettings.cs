using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectLapShop.Models
{
    public class TbSettings
    {
        [Key]
        public int Id {  get; set; }
        public string WebsiteName { get; set; } = "";
        public string WebsiteDescription { get; set; } = "";
        public string Logo { get; set; } = "";
        public string FacebookLink { get; set; } = "";
        public string TwiterLink { get; set; } = "";
        public string InstgramLink { get; set; } = "";
        public string YoutubeLink { get; set; } = "";
        public string GoogleLink { get; set; } = "";
        public string Location { get; set; } = "";
        public string CallUs { get; set; } = "";
        public string EmailWebsite { get; set; } = "";
        public string Fax { get; set; } = "";
        public string ImageWebsite { get; set; } = "";
        public string MessageWelcom { get; set; } = "";
        public string WebsiteDescriptionCenter { get; set; } = "";
        public string MainName1 { get; set; } = "";
        public string MainName2 { get; set; } = "";
        public string MainName3 { get; set; } = "";
        public string MainName4 { get; set; } = "";
        public string FooterWelcom { get; set; } = "";
        public string FooterDescription { get; set; } = "";
    }
}
