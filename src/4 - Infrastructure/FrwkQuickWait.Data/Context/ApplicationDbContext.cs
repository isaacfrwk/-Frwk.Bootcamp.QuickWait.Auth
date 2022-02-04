using FrwkQuickWait.Data.Mapping;
using FrwkQuickWait.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FrwkQuickWait.Data.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options){ }

        public virtual DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new UserMap());
        }
    }
}
