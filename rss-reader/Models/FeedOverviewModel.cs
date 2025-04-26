namespace rss_reader.Models;

public class FeedOverviewModel
{
    public Guid FeedId { get; set; }
    public string Name { get; set; } = null!;
}