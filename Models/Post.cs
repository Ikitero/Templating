using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Templating.ViewModels;

namespace Templating.Models
{
    public class Post
    {
        public int Id { get; set; }
        public IEnumerable<Template> Templates { get; set; }
    }
}
