using FrwkQuickWait.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FrwkQuickWait.Infrastructure.Data.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options){ }

        public virtual DbSet<User> Users { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder) =>
           builder.Entity<User>().ToTable("users");
    }
}
