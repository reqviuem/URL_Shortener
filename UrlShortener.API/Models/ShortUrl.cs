namespace UrlShortener.API.Models;

public class ShortUrl
{
    public int Id { get; set; }
    public string OriginalUrl { get; set; } = string.Empty;
    public string ShortenedUrl { get; set; } = string.Empty;
    public string CreatedBy { get; set; } = string.Empty; 
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
}