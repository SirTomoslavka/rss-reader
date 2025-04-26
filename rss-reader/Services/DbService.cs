using Microsoft.EntityFrameworkCore;
using rss_reader.Data;

public class DbService(ApplicationDbContext db)
{
    public async Task<Feed?> GetFeedWithArticlesAsync(Guid feedId)
    {
        return await db.Feeds
            .Where(f => f.FeedId == feedId)
            .Select(f => new Feed
            {
                FeedId = f.FeedId,
                Name = f.Name,
                Url = f.Url,
                Description = f.Description,
                Language = f.Language,
                ImageUrl = f.ImageUrl,
                Articles = f.Articles
                    .OrderByDescending(a => a.Published)
                    .ToList()
            })
            .FirstOrDefaultAsync();
    }

    public async Task<Feed?> GetFeedWithArticlesAsync(Guid feedId, DateTime from, DateTime to)
    {
        return await db.Feeds
            .Where(f => f.FeedId == feedId)
            .Select(f => new Feed
            {
                FeedId = f.FeedId,
                Name = f.Name,
                Url = f.Url,
                Description = f.Description,
                Language = f.Language,
                ImageUrl = f.ImageUrl,
                Articles = f.Articles
                    .Where(a => a.Published >= from.ToUniversalTime() && a.Published <= to.ToUniversalTime())
                    .OrderByDescending(a => a.Published)
                    .ToList()
            })
            .FirstOrDefaultAsync();
    }
    
    public async Task<Article?> GetArticleAsync(Guid articleId)
    {
        return await db.Articles.FindAsync(articleId);
    }

    public async Task<List<Feed>> GetAllFeedsAsync()
    {
        return await db.Feeds.AsNoTracking().ToListAsync();
    }
    
    public async Task<Feed?> GetFeedAsync(Guid feedId)
    {
        return await db.Feeds.FindAsync(feedId);
    }

    public async Task AddFeedAsync(Feed feed)
    {
        db.Feeds.Add(feed);
        await db.SaveChangesAsync();
    }

    public async Task RemoveFeedAsync(Guid feedId)
    {
        var feed = await db.Feeds.FindAsync(feedId);
        if (feed is not null)
        {
            db.Feeds.Remove(feed);
            await db.SaveChangesAsync();
        }
    }

    public async Task AddArticlesAsync(IEnumerable<Article> articles)
    {
        db.Articles.AddRange(articles);
        await db.SaveChangesAsync();
    }
}
