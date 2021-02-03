using Microsoft.EntityFrameworkCore;
using Abp.Zero.EntityFrameworkCore;
using FirstProject.Authorization.Roles;
using FirstProject.Authorization.Users;
using FirstProject.MultiTenancy;
using FirstProject.Books;
using FirstProject.Categories;
using FirstProject.Publishers;
using FirstProject.BookCategories;

namespace FirstProject.EntityFrameworkCore
{
    public class FirstProjectDbContext : AbpZeroDbContext<Tenant, Role, User, FirstProjectDbContext>
    {
        public DbSet<Book> Books { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Publisher> Publishers { get; set; }
        public DbSet<BookCategory> BookCategories { get; set; }
        public FirstProjectDbContext(DbContextOptions<FirstProjectDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<BookCategory>()
                .HasKey(bc => new { bc.BookId, bc.CategoryId });
        }
    }
}
