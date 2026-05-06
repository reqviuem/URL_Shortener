using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using UrlShortener.API.Data;
using UrlShortener.API.Models;

namespace UrlShortener.API.Pages
{
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

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var aboutData = await _context.AboutInfo.FirstOrDefaultAsync();

            if (aboutData == null)
            {
                aboutData = new About { Description = NewDescription };
                _context.AboutInfo.Add(aboutData);
            }
            else
            {
                aboutData.Description = NewDescription;
                _context.AboutInfo.Update(aboutData);
            }

            await _context.SaveChangesAsync();

            return RedirectToPage();
        }
    }
}