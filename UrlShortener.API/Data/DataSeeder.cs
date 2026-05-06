using Microsoft.AspNetCore.Identity;
using UrlShortener.API.Models;

namespace UrlShortener.API.Data;

public static class DataSeeder
{
    public static async Task SeedUsersAsync(IServiceProvider serviceProvider)
    {
        var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        if (await userManager.FindByNameAsync("admin") == null)
        {
            var adminUser = new ApplicationUser { UserName = "admin", Admin = true };
            var result = await userManager.CreateAsync(adminUser, "Secret123!");

            if (!result.Succeeded)
                throw new Exception(
                    $"Failed to create admin: {string.Join(", ", result.Errors.Select(e => e.Description))}");
        }

        if (await userManager.FindByNameAsync("Viacheslav") == null)
        {
            var normalUser = new ApplicationUser { UserName = "Viacheslav", Admin = false };
            var result = await userManager.CreateAsync(normalUser, "Pass123!");

            if (!result.Succeeded)
                throw new Exception(
                    $"Failed to create Viacheslav: {string.Join(", ", result.Errors.Select(e => e.Description))}");
        }
        
    }
}