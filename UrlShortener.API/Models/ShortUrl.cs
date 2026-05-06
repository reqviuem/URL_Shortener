namespace UrlShortener.API.Models;

public class ShortUrl
{
    public int Id { get; set; }
    public string OriginalUrl { get; set; }
    public string ShortCode { get; set; }
    public string CreatedById { get; set; }
    public DateTime CreatedDate { get; set; }

    public string CreatedBy { get; set; }
}