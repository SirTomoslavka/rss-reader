using System.ComponentModel.DataAnnotations;

namespace rss_reader.Data;

public class Article
{
    [Required]
    public Guid ArticleId { get; init; }
    
    [Required]
    public string Title { get; init; } = null!;
    [Required]
    public DateTime Published { get; init; } = DateTime.UtcNow;
    [Required]
    public string Link { get; init; } = null!;

    [Required]
    public Guid FeedId { get; set; }
    [Required]
    public Feed Feed { get; set; } = null!;
}