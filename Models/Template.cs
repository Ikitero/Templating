using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Templating.ViewModels;

namespace Templating.Models
{
    public class Template
    {
        public int Id { get; set; }
        public string Slug { get; set; }
        public string Json { get; set; }
        public int PostId { get; set; }
    }
}
