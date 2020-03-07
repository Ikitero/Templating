using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Templating.Models;
using Templating.ViewModels;

namespace Templating.Models
{
    public class PostRepository
    {
        private readonly AppDbContext _context;
        public PostRepository(AppDbContext context)
        {
            _context = context;
        }
        public List<Post> Posts()
        {
            var posts = _context.Posts.ToList();
            foreach(var post in posts)
            {
                post.Templates = _context.Templates.Where(t => t.PostId == post.Id).ToList<Template>();
            }
            return posts;
        }
        public void RemovePost(int postId)
        {
            var record = _context.Posts.FirstOrDefault(r => r.Id == postId);
            _context.Remove(record);
            _context.SaveChanges();
        }
        public void CreatePost(string data)
        {
            var post = _context.Posts.OrderByDescending(p => p.Id).FirstOrDefault();
            List<TemplateViewModel> templates = JsonConvert.DeserializeObject<List<TemplateViewModel>>(data);          
            foreach(var template in templates)
            {
                template.DateCreated = DateTime.Now;
                template.DateModified = DateTime.Now;
                _context.Templates.Add(new Template{ Json = JsonConvert.SerializeObject(template), Slug = template.Slug, PostId = post.Id });
            }
            _context.SaveChanges();
        }
       
    }
}
