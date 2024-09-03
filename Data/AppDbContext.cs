using System;
using System.Collections.Concurrent;
using System.Linq;
using Backend.Modals;
using DalDocBackend.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DalDocBackend.Data
{
    public class AppDbContext : IdentityDbContext<User>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Departments> Departments { get; set; }
        public DbSet<Course> Courses { get; set; }

         public DbSet<Comment> Comments { get; set; }

    }
}
