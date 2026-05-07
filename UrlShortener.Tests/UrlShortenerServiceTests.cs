using FluentAssertions;
using Xunit;
using UrlShortener.API.Services;

namespace UrlShortener.Tests.Unit
{
    public class UrlShortenerServiceTests
    {
        [Fact]
        public void GenerateShortUrl_ShouldReturn_Exactly6Characters()
        {
            
            var service = new UrlShortenerService(null!); 
            
            var result = service.GenerateShortUrl();
            
            result.Should().NotBeNullOrWhiteSpace();
            result.Length.Should().Be(6);
        }

        [Fact]
        public void GenerateShortUrl_ShouldGenerate_UniqueStrings()
        {
            var service = new UrlShortenerService(null!);
            
            var result1 = service.GenerateShortUrl();
            var result2 = service.GenerateShortUrl();
            
            result1.Should().NotBe(result2);
        }
    }
}