using Microsoft.EntityFrameworkCore;
using System.Configuration;

namespace ArticleApp.Models
{
    public class ArticleAppDbContext : DbContext
    {
        public ArticleAppDbContext()
        {
            //Empty
        }

        public ArticleAppDbContext(DbContextOptions<ArticleAppDbContext> options)
            :base(options) 
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
            modelBuilder.Entity<Article>().Property(m => m.Created).HasDefaultValueSql("GetDate()");
        }

        public DbSet<Article> Articles { get; set; }
    }
}
