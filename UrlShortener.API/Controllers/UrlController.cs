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
    public async Task<IActionResult> GetAllUrls()
    {
        var urls = await _urlService.GetAllUrlsAsync();
        return Ok(urls);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetUrl(int id)
    {
        var url = await _urlService.GetUrlByIdAsync(id);
        if (url == null) return NotFound(new { message = "URL not found." });

        return Ok(url);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CreateUrl([FromBody] CreateUrlDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.OriginalUrl))
            return BadRequest(new { message = "URL is required." });

        var username = User.FindFirstValue(ClaimTypes.Name);
        var newUrl = await _urlService.CreateUrlAsync(dto.OriginalUrl, username);

        return Ok(newUrl);
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> DeleteUrl(int id)
    {
        var username = User.FindFirstValue(ClaimTypes.Name);
        var isAdmin = User.IsInRole("Admin");

        try
        {
            var success = await _urlService.DeleteUrlAsync(id, username, isAdmin);
            if (!success) return NotFound(new { message = "URL not found." });

            return Ok();
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
    }

    [HttpGet("{shortUrl}")]
    public async Task<IActionResult> RedirectToOriginal(string shortUrl)
    {
        var urlEntity = await _urlService.GetByShortUrlAsync(shortUrl);
        
        if (urlEntity == null)
            return NotFound("Short URL not found");

        return Redirect(urlEntity.OriginalUrl);
    }
}