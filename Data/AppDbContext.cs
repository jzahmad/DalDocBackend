using System;
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

        public override int SaveChanges()
        {
            var entries = ChangeTracker.Entries<User>()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

            foreach (var entry in entries)
            {
                if (!entry.Entity.Email.EndsWith("@dal.ca"))
                {
                    throw new InvalidOperationException("Email should end with '@dal'.");
                }
            }

            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker.Entries<User>()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

            foreach (var entry in entries)
            {
                if (!entry.Entity.Email.EndsWith("@dal.ca"))
                {
                    throw new InvalidOperationException("Email should end with '@dal'.");
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
