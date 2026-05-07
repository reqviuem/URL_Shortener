using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UrlShortener.API.Controllers;
using UrlShortener.API.Data;
using UrlShortener.API.Models;
using UrlShortener.API.Services;
using Xunit;

namespace UrlShortener.Tests.Unit
{
    public class UrlControllerTests
    {
        private async Task<AppDbContext> GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var context = new AppDbContext(options);
            await context.Database.EnsureCreatedAsync();
            return context;
        }

        [Fact]
        public async Task RedirectToOriginal_ShouldReturnNotFound_WhenUrlDoesNotExist()
        {
            var context = await GetInMemoryDbContext();

            var service = new UrlShortenerService(context);

            var controller = new UrlController(service);

            var result = await controller.RedirectToOriginal("invalid");

            result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async Task RedirectToOriginal_ShouldReturnRedirect_WhenUrlExists()
        {
            var context = await GetInMemoryDbContext();
            var fakeUrl = new ShortUrl
            {
                OriginalUrl = "https://google.com",
                ShortenedUrl = "a1b2c3",
                CreatedBy = "admin"
            };

            context.ShortUrls.Add(fakeUrl);
            await context.SaveChangesAsync();

            var service = new UrlShortenerService(context);

            var controller = new UrlController(service);

            var result = await controller.RedirectToOriginal("a1b2c3");

            var redirectResult = result.Should().BeOfType<RedirectResult>().Subject;
            redirectResult.Url.Should().Be("https://google.com");
        }
    }
}