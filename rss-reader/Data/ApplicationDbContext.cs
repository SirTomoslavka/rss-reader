using Microsoft.EntityFrameworkCore;

namespace rss_reader.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Feed> Feeds { get; set; }
        public DbSet<Article> Articles { get; set; }
        
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Feed>(entity =>
            {
                entity.HasKey(f => f.FeedId);

                entity.Property(f => f.Name)
                      .IsRequired();

                entity.Property(f => f.Url)
                      .IsRequired();

                entity.Property(f => f.Description)
                      .IsRequired();

                entity.Property(f => f.Language)
                      .IsRequired();

                entity.Property(f => f.ImageUrl)
                      .IsRequired();

                entity.HasMany(f => f.Articles)
                      .WithOne(a => a.Feed)
                      .HasForeignKey(a => a.FeedId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(f => f.Name);
                
                entity.HasIndex(f => f.Url)
                      .IsUnique();
            });

            modelBuilder.Entity<Article>(entity =>
            {
                entity.HasKey(a => a.ArticleId);

                entity.Property(a => a.Title)
                      .IsRequired();

                entity.Property(a => a.Link)
                      .IsRequired();

                entity.Property(a => a.Published)
                      .IsRequired();

                entity.HasIndex(a => a.Published);
            });
        }
    }
}
