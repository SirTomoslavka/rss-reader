using System.ComponentModel.DataAnnotations;

namespace rss_reader.Models;

public class CreateFeedModel
{
    [Required, Url]
    public string Url  { get; set; } = null!;

    [MaxLength(200)] 
    public string? Name { get; set; } = null!;
}