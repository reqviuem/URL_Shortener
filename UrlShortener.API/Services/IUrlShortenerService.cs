using UrlShortener.API.Models;

namespace UrlShortener.API.Services;

public interface IUrlShortenerService
{
    Task<IEnumerable<ShortUrl>> GetAllUrlsAsync(CancellationToken cancellationToken = default);
    Task<ShortUrl?> GetUrlByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<ShortUrl?> GetByShortUrlAsync(string shortUrl, CancellationToken cancellationToken = default);
    Task<ShortUrl> CreateUrlAsync(string originalUrl, string createdBy, CancellationToken cancellationToken = default);
    Task<bool> DeleteUrlAsync(int id, string currentUsername, bool isAdmin, CancellationToken cancellationToken = default);
}