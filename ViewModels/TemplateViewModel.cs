using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Templating.ViewModels
{
    public class TemplateViewModel
    {
        public string MainHeader { get; set; }
        public string SubHeader { get; set; }
        public string ShortDescription { get; set; }
        public string Description { get; set; }
        public IEnumerable<string> ImageUrl { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public string Status { get; set; }
        public float RegularPrice { get; set; }
        public float SalePrice { get; set; }
        public bool IsShippingRequired { get; set; }
        public string Sku { get; set; }
        public string Slug { get; set; }
        public string __RequestVerificationToken { get; set; }
    }
}
