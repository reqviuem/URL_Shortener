using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UrlShortener.API.DTOs;
using UrlShortener.API.Services;

namespace UrlShortener.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UrlController : ControllerBase
{
    private readonly IUrlShortenerService _urlService;

    public UrlController(IUrlShortenerService urlService)
    {
        _urlService = urlService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllUrls(CancellationToken cancellationToken)
    {
        var urls = await _urlService.GetAllUrlsAsync(cancellationToken);
        return Ok(urls);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetUrl(int id, CancellationToken cancellationToken)
    {
        var url = await _urlService.GetUrlByIdAsync(id, cancellationToken);
        if (url == null) return NotFound(new { message = "URL not found." });

        return Ok(url);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CreateUrl([FromBody] CreateUrlDto dto, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(dto.OriginalUrl))
            return BadRequest(new { message = "URL is required." });

        var username = User.FindFirstValue(ClaimTypes.Name);
        var newUrl = await _urlService.CreateUrlAsync(dto.OriginalUrl, username, cancellationToken);

        return Ok(newUrl);
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> DeleteUrl(int id, CancellationToken cancellationToken)
    {
        var username = User.FindFirstValue(ClaimTypes.Name);
        var isAdmin = User.IsInRole("Admin");

        try
        {
            var success = await _urlService.DeleteUrlAsync(id, username, isAdmin, cancellationToken);
            if (!success) return NotFound(new { message = "URL not found." });

            return Ok();
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
    }

    [HttpGet("{shortUrl}")]
    public async Task<IActionResult> RedirectToOriginal(string shortUrl, CancellationToken cancellationToken)
    {
        var urlEntity = await _urlService.GetByShortUrlAsync(shortUrl, cancellationToken);
        
        if (urlEntity == null)
            return NotFound("Short URL not found");

        return Redirect(urlEntity.OriginalUrl);
    }
}