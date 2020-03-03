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
            List<TemplateViewModel> templates = JsonWorker(data);
            foreach(var template in templates)
            {
                _context.Templates.Add(new Template{ Json = JsonConvert.SerializeObject(template), Slug = template.Slug, PostId = post.Id });
            }
            _context.SaveChanges();
        }
        public List<TemplateViewModel> JsonWorker(string data)
        {
            string[] chunks = data.Split('{', '}', '[', ']');
            List<string> lines = new List<string>();
            foreach (var s in chunks)
            {
                if (s == "" || s.Trim(',', '\"') == "" || s.Contains("Token")) { continue; }
                else { lines.Add(s.Replace(",", " ").Replace("\"", " ").Trim()); }
            }
            string formName = "";
            List<TemplateViewModel> models = new List<TemplateViewModel>();
            for (int i = 0; i < lines.Count; i++)
            {
                if (lines[i].Contains("templateName"))
                {
                    formName = lines[i].Split(" ")[1];
                    models.Add(GetModel(lines, formName, i));
                }
            }
            return models;
        }

        private TemplateViewModel GetModel(List<string> lines, string formName, int index)
        {
            TemplateViewModel model = new TemplateViewModel();
            model.Slug = formName;
            for (int i = index; i < lines.Count; i++)
            {
                if (lines[i].Contains("templateName") && !lines[i].Contains(formName))
                {
                    return model;
                }
                else if (lines[i].Contains("templateName")) { continue; }
                else
                {
                    string[] tmp = new string[2];
                    tmp[0] = lines[i].Split("   ")[0];
                    tmp[1] = lines[i].Split("   ")[1];
                    if (lines[i].Split("   ")[0].Contains("Url"))
                    {
                        model.SetAttribute<IEnumerable<string>>(tmp[0].Split(" : ")[1].Trim(), LoadImages(lines, ref i));
                        if (i >= lines.Count) return model;
                        else
                        {
                            continue;
                        }
                    }
                    model.SetAttribute<string>(tmp[0].Split(" : ")[1].Trim(), tmp[1].Split(" : ")[1].Trim());
                }
            }
            return model;
        }
        private IEnumerable<string> LoadImages(List<string> lines, ref int index)
        {
            List<string> images = new List<string>();

            while (index < lines.Count && lines[index].Split("   ")[0].Contains("Url"))
            {
                images.Add(lines[index].Split("   ")[1].Split(" : ")[1].Trim());
                index++;
            }
            index--;
            return images;
        }
    }
}
