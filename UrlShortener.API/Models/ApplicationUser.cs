using Microsoft.AspNetCore.Identity;

namespace UrlShortener.API.Models;

public class ApplicationUser : IdentityUser
{
    public bool Admin { get; set; }
}