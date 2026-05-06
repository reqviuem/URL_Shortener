using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UrlShortener.API.Data;
using UrlShortener.API.Models;

namespace UrlShortener.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AboutController : ControllerBase
{
    private readonly AppDbContext _context;

    public AboutController(AppDbContext context)
    {
        _context = context;
    }
    
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAbout()
    {
        var about = await _context.Abouts.FirstOrDefaultAsync();
        return Ok(about);
    }
    
    [HttpPut]
    [Authorize]
    public async Task<IActionResult> EditAbout([FromBody] About updatedAbout)
    {
        var isAdmin = User.Claims.FirstOrDefault(c => c.Type == "isAdmin")?.Value == "True";
        if (!isAdmin)
        {
            return Forbid();
        }

        var about = await _context.Abouts.FirstOrDefaultAsync();
        if (about == null) return NotFound();

        about.Description = updatedAbout.Description;
        await _context.SaveChangesAsync();

        return Ok(about);
    }
}