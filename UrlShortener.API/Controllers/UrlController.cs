using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UrlShortener.API.Data;
using UrlShortener.API.DTOs;
using UrlShortener.API.Models;
using UrlShortener.API.Services;

namespace UrlShortener.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UrlController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly UrlShortenerService _urlService;

    public UrlController(AppDbContext context, UrlShortenerService urlService)
    {
        _context = context;
        _urlService = urlService;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll()
    {
        var urls = await _context.ShortUrls.ToListAsync();
        return Ok(urls);
    }

    [HttpGet("{id:int}")]
    [Authorize]
    public async Task<IActionResult> GetById(int id)
    {
        var url = await _context.ShortUrls.FindAsync(id);
        if (url == null) return NotFound();

        bool isAdmin = User.IsInRole("Admin") || User.HasClaim(c => c.Type == "isAdmin" && c.Value.ToLower() == "true");

        string currentUsername = User.Identity?.Name ?? "";

        if (!isAdmin && url.CreatedBy != currentUsername)
        {
            return Forbid();
        }

        return Ok(url);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create([FromBody] CreateUrlDto dto)
    {
        var existing = await _urlService.GetByOriginalUrlAsync(dto.OriginalUrl);
        if (existing != null)
        {
            return BadRequest(new { message = "This URL has already been shortened." });
        }

        var username = User.Identity?.Name ?? "Unknown";

        var newUrl = new ShortUrl
        {
            OriginalUrl = dto.OriginalUrl,
            ShortenedUrl = _urlService.GenerateShortUrl(),
            CreatedBy = username,
            CreatedDate = DateTime.UtcNow
        };

        _context.ShortUrls.Add(newUrl);
        await _context.SaveChangesAsync();

        return Ok(newUrl);
    }

    [HttpGet("{shortUrl}")]
    [AllowAnonymous]
    public async Task<IActionResult> RedirectToOriginal(string shortUrl)
    {
        var urlEntity = await _context.ShortUrls.FirstOrDefaultAsync(u => u.ShortenedUrl == shortUrl);

        if (urlEntity == null)
        {
            return NotFound("Short URL not found.");
        }

        return Redirect(urlEntity.OriginalUrl);
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> Delete(int id)
    {
        var url = await _context.ShortUrls.FindAsync(id);
        if (url == null) return NotFound();

        var currentUsername = User.Identity?.Name;
        var isAdmin = User.Claims.FirstOrDefault(c => c.Type == "isAdmin")?.Value == "True";

        if (!isAdmin && url.CreatedBy != currentUsername)
        {
            return Forbid();
        }

        _context.ShortUrls.Remove(url);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}