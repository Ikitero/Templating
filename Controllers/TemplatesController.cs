using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Templating.Models;
using Templating.ViewModels;

namespace Templating.Controllers
{
    public class TemplatesController : Controller
    {
        private readonly AppDbContext _context;
        public TemplatesController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult AddingList()
        {
            return View();
        }

        [HttpPost]
        public PartialViewResult Load(string templateName)
        {
            TemplateViewModel model = new TemplateViewModel();
            model.Slug = templateName;
            return PartialView(templateName,model);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(TemplateViewModel model)
        {
            if (ModelState.IsValid && model != null)
            {
                Template template = new Template();
                template.Slug = model.Slug;
                model.DateCreated = DateTime.Now;
                model.DateModified = DateTime.Now;
                string json = JsonConvert.SerializeObject(model);
                template.Json = json;
                _context.Templates.Add(template);
                await _context.SaveChangesAsync();
            }
            return View();
        }
    }
}
