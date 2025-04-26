using System.ServiceModel.Syndication;
using System.Xml;
using rss_reader.Data;
using rss_reader.Models;

namespace rss_reader.Services;

public class RssService(DbService db)
{
    public async Task<MainPageViewModel> GetAllFeedsAsync()
    {
        var feeds = await db.GetAllFeedsAsync();

        MainPageViewModel model = new();

        foreach (var feed in feeds)
        {
            model.Feeds.Add(new FeedOverviewModel()
            {
                FeedId = feed.FeedId,
                Name = feed.Name,
            });
        }
        
        return model;
    }

    public Task<Feed?> GetFeedWithArticlesAsync(Guid feedId)
    {
        return db.GetFeedWithArticlesAsync(feedId);
    }

    public async Task<FeedDetailViewModel?> GetFeedAsync(Guid feedId)
    {
        var feed = await db.GetFeedAsync(feedId);

        if (feed != null)
            return new FeedDetailViewModel()
            {
                FeedId = feed.FeedId,
                Name = feed.Name,
                Description = feed.Description,
                Language = feed.Language,
                ImageUrl = feed.ImageUrl,
                Url = feed.Url,
            };
        
        return null;
    }

    public string? GetArticleUrl(Guid articleId)
    {
        var article = db.GetArticleAsync(articleId).GetAwaiter().GetResult();

        return article?.Link;
    }

    public Task<Feed?> GetFeedWithArticlesAsync(Guid feedId, DateTime from, DateTime to)
    {
        return db.GetFeedWithArticlesAsync(feedId, from, to);
    }

    public async Task<Feed> AddFeedAndFetchArticlesAsync(string feedUrl, string? name)
    {
        using var reader = XmlReader.Create(feedUrl);
        var syndicationFeed = SyndicationFeed.Load(reader) ?? throw new InvalidOperationException("Invalid RSS feed");

        Guid feedId = Guid.NewGuid();

        var feed = new Feed
        {
            FeedId = feedId,
            Name = name ?? syndicationFeed.Title.Text,
            Url = syndicationFeed.Links[0].Uri.ToString(),
            Description = syndicationFeed.Description.Text,
            Language = syndicationFeed.Language,
            ImageUrl = syndicationFeed.ImageUrl?.AbsoluteUri ?? string.Empty,
            Articles = syndicationFeed.Items.Select(item => new Article
            {
                ArticleId = Guid.NewGuid(),
                Title = item.Title.Text,
                Link = item.Links.FirstOrDefault()?.Uri.ToString() ?? string.Empty,
                Published = item.PublishDate.DateTime.ToUniversalTime(),
                FeedId = feedId
            }).ToList()
        };
        

        await db.AddFeedAsync(feed);

        return feed;
    }

    public async Task<List<Article>> ReloadNewArticlesAsync(Guid feedId)
    {
        var feed = await db.GetFeedWithArticlesAsync(feedId)
                   ?? throw new KeyNotFoundException($"Feed {feedId} not found");

        using var reader     = XmlReader.Create(feed.Url);
        var syndicationFeed  = SyndicationFeed.Load(reader)
                               ?? throw new InvalidOperationException("Invalid RSS feed");

        var existingLinks = new HashSet<string?>(feed.Articles.Select(a => a.Link));
        var newArticles= syndicationFeed.Items
            .Where(i=> !existingLinks.Contains(i.Links.FirstOrDefault()?.Uri.ToString()))
            .Select(item => new Article
            {
                ArticleId = Guid.NewGuid(),
                Title = item.Title.Text,
                Link = item.Links.First().Uri.ToString(),
                Published = item.PublishDate.DateTime.ToUniversalTime(),
                FeedId = feed.FeedId
            })
            .ToList();

        if (newArticles.Count > 0)
            await db.AddArticlesAsync(newArticles);

        return newArticles;
    }

    public async Task DeleteFeedsAsync(string feedsIds)
    {
        var feedIds = feedsIds.Split(',').Select(Guid.Parse).ToList();
        
        foreach (var id in feedIds)
            await db.RemoveFeedAsync(id);
    }
}