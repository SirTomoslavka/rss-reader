using System.ComponentModel.DataAnnotations;

namespace rss_reader.Data;

public sealed class Feed
{
    public Guid FeedId { get; set; }

    [Required] 
    public string Name { get; set; } = null!;
    
    [Required, Url]
    public string Url { get; set; } = null!;

    [Required]
    public string Description { get; set; } = null!;
    [Required]
    public string Language { get; set; } = null!;
    [Required, Url]
    public string ImageUrl { get; set; } = null!;

    public ICollection<Article> Articles { get; set; } = new List<Article>();
}