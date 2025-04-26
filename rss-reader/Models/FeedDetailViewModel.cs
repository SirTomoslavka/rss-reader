namespace rss_reader.Models;

public class FeedDetailViewModel
{
    public Guid FeedId { get; set; }
    
    public string Name { get; set; } = null!;
    
    public string Language { get; set; } = null!;

    public string Url { get; set; } = null!;
    
    public string Description { get; set; } = null!;

    public string ImageUrl { get; set; } = null!;
}