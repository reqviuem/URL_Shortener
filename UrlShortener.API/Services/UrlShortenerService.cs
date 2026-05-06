using Microsoft.EntityFrameworkCore;
using UrlShortener.API.Data;
using UrlShortener.API.Models;

namespace UrlShortener.API.Services;

public class UrlShortenerService
{
    private readonly AppDbContext _context;

    public UrlShortenerService(AppDbContext context)
    {
        _context = context;
    }
    
    public string GenerateShortUrl()
    {
        // Creates a random 6 character string ("aB3x9Y")
        return Guid.NewGuid().ToString().Substring(0, 6);
    }

    public async Task<ShortUrl?> GetByOriginalUrlAsync(string originalUrl)
    {
        return await _context.ShortUrls.FirstOrDefaultAsync(u => u.OriginalUrl == originalUrl);
    }
}