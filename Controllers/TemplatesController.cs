using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Templating.Models;
using Templating.ViewModels;

namespace Templating.Controllers
{
    public class TemplatesController : Controller
    {
        private readonly AppDbContext _context;
        private PostRepository _postRepository;
        public TemplatesController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> List()
        {
            _postRepository = new PostRepository(_context);
            var post = _postRepository.Posts();
            List<List<TemplateViewModel>> models = new List<List<TemplateViewModel>>();
            foreach (var t in post)
            {
                List<TemplateViewModel> tmp = new List<TemplateViewModel>();
                foreach (var m in t.Templates)
                {
                    tmp.Add(JsonConvert.DeserializeObject<TemplateViewModel>(m.Json));
                }
                models.Add(tmp);
            }
            Tuple<List<Post>, List<List<TemplateViewModel>>> model = new Tuple<List<Post>, List<List<TemplateViewModel>>>(post, models);
            return View("List", model);
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
        public async Task<JsonResult> Create(string data)
        {
            if (ModelState.IsValid)
            {
                _postRepository = new PostRepository(_context);
                Post post = new Post();
                await _context.Posts.AddAsync(post);
                await _context.SaveChangesAsync();
                _postRepository.CreatePost(data);
            }
            return Json(new { result = "Redirect", url = Url.Action("List", "Templates") });
        }

        public IActionResult Remove(int postId)
        {
            _postRepository = new PostRepository(_context);
            _postRepository.RemovePost(postId);
            return View("Create");
        }
    }
}
