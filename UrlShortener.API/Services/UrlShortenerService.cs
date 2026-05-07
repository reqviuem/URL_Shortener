using Microsoft.EntityFrameworkCore;
using UrlShortener.API.Data;
using UrlShortener.API.Models;

namespace UrlShortener.API.Services;

public class UrlShortenerService : IUrlShortenerService
{
    private readonly AppDbContext _context;

    public UrlShortenerService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ShortUrl>> GetAllUrlsAsync(CancellationToken cancellationToken = default)
    {
        return await _context.ShortUrls.ToListAsync(cancellationToken);
    }

    public async Task<ShortUrl?> GetUrlByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.ShortUrls.FindAsync(new object[] { id }, cancellationToken);
    }

    public async Task<ShortUrl?> GetByShortUrlAsync(string shortUrl, CancellationToken cancellationToken = default)
    {
        return await _context.ShortUrls.FirstOrDefaultAsync(u => u.ShortenedUrl == shortUrl, cancellationToken);
    }

    public async Task<ShortUrl> CreateUrlAsync(string originalUrl, string createdBy, CancellationToken cancellationToken = default)
    {
        var shortUrl = new ShortUrl
        {
            OriginalUrl = originalUrl,
            ShortenedUrl = GenerateShortUrl(),
            CreatedBy = createdBy,
            CreatedDate = DateTime.UtcNow
        };

        _context.ShortUrls.Add(shortUrl);
        await _context.SaveChangesAsync(cancellationToken);

        return shortUrl;
    }

    public async Task<bool> DeleteUrlAsync(int id, string currentUsername, bool isAdmin, CancellationToken cancellationToken = default)
    {
        var urlEntity = await _context.ShortUrls.FindAsync(new object[] { id }, cancellationToken);
        if (urlEntity == null) return false;

        if (!isAdmin && urlEntity.CreatedBy != currentUsername)
        {
            throw new UnauthorizedAccessException("You don't have permission to delete this URL.");
        }

        _context.ShortUrls.Remove(urlEntity);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

    private string GenerateShortUrl()
    {
        return Guid.NewGuid().ToString().Substring(0, 6);
    }
}