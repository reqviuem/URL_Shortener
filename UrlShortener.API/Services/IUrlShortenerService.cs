using UrlShortener.API.Models;

namespace UrlShortener.API.Services;

public interface IUrlShortenerService
{
    Task<IEnumerable<ShortUrl>> GetAllUrlsAsync();
    Task<ShortUrl?> GetUrlByIdAsync(int id);
    Task<ShortUrl?> GetByShortUrlAsync(string shortUrl);
    Task<ShortUrl> CreateUrlAsync(string originalUrl, string createdBy);
    Task<bool> DeleteUrlAsync(int id, string currentUsername, bool isAdmin);
}