using Microsoft.EntityFrameworkCore;
using System.Configuration;

namespace NoticeApp.Models
{
    public class NoticeAppDbContext : DbContext
    {
        public NoticeAppDbContext()
        {

        }

        public NoticeAppDbContext(DbContextOptions<NoticeAppDbContext> options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if(!optionsBuilder.IsConfigured)
            {
                string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                optionsBuilder.UseSqlServer(connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Notice>().Property(m => m.Created).HasDefaultValueSql("GETDATE()");
        }

        public DbSet<Notice> Notices { get; set; }
    }
}
