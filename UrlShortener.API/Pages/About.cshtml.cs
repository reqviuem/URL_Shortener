
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using UrlShortener.API.Data;
using UrlShortener.API.Models;

namespace UrlShortener.API.Pages
{
    [IgnoreAntiforgeryToken] 
    public class AboutModel : PageModel
    {
        private readonly AppDbContext _context;

        public AboutModel(AppDbContext context)
        {
            _context = context;
        }

        public string AlgorithmDescription { get; set; } = string.Empty;

        [BindProperty] public string NewDescription { get; set; } = string.Empty;

        public async Task OnGetAsync()
        {
            var aboutData = await _context.AboutInfo.FirstOrDefaultAsync();

            AlgorithmDescription = aboutData?.Description ??
                                   "URL shortener takes a long URL, generates a unique 6-character string, and stores it in the database";
            NewDescription = AlgorithmDescription;
        }
        
        public async Task<IActionResult> OnPostAsync(string NewDescription)
        {
            if (!User.Identity.IsAuthenticated || !(User.IsInRole("Admin") || User.HasClaim(c => c.Type == "isAdmin" && c.Value.ToLower() == "true")))
            {
                return Forbid();
            }

            var aboutData = await _context.AboutInfo.FirstOrDefaultAsync();

            if (aboutData == null)
            {
                aboutData = new About { Description = NewDescription ?? "Default Description" };
                _context.AboutInfo.Add(aboutData);
            }
            else
            {
                aboutData.Description = NewDescription ?? "Default Description";
                _context.AboutInfo.Update(aboutData);
            }

            await _context.SaveChangesAsync();
            
            return RedirectToPage(new { access_token = Request.Query["access_token"] });
        }
    }
}