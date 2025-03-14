using ass_api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ass_api.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public DbSet<Product> Products { get; set; }
        public DbSet<Loai> Loais { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<HoaDon> HoaDons { get; set; }
        public DbSet<HoaDonDetail> HoaDonDetails { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Cấu hình quan hệ giữa Loai và Product
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Loai)
                .WithMany(l => l.Products)
                .HasForeignKey(p => p.LoaiID)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
