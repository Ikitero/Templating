using Microsoft.AspNetCore.Mvc;
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

        public IActionResult List()
        {
            _postRepository = new PostRepository(_context);
            var post = _postRepository.Posts;
            return View("List", post);
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

        public IActionResult Create(int id)
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(string data)
        {

            if (ModelState.IsValid)
            {
                _postRepository = new PostRepository(_context);
                Post post = new Post();
                await _context.Posts.AddAsync(post);
                await _context.SaveChangesAsync();
                _postRepository.CreatePost(data);
            }
            return View("Create");
        }
    }
}
