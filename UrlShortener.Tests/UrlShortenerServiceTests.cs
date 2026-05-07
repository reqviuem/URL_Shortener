using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using UrlShortener.API.Data;
using UrlShortener.API.Services;
using Xunit;

namespace UrlShortener.Tests.Unit
{
    public class UrlShortenerServiceTests
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
        public async Task CreateUrlAsync_ShouldGenerate_Exactly6CharacterShortUrl()
        {
            var context = await GetInMemoryDbContext();
            var service = new UrlShortenerService(context);
            
            var result = await service.CreateUrlAsync("https://apple.com", "admin");
            
            result.ShortenedUrl.Should().NotBeNullOrWhiteSpace();
            result.ShortenedUrl.Length.Should().Be(6);
        }

        [Fact]
        public async Task CreateUrlAsync_ShouldGenerate_UniqueShortUrls()
        {
            var context = await GetInMemoryDbContext();
            var service = new UrlShortenerService(context);
            
            var result1 = await service.CreateUrlAsync("https://apple.com", "admin");
            var result2 = await service.CreateUrlAsync("https://google.com", "admin");
            
            result1.ShortenedUrl.Should().NotBe(result2.ShortenedUrl);
        }
    }
}