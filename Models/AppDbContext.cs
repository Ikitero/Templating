using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Templating.Models
{
    public class AppDbContext : IdentityDbContext<IdentityUser>
    {
        public DbSet<Template> Templates { get; set; }
        public DbSet<Post> Posts { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
    }
}
