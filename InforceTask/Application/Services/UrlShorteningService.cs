using System.Security.Cryptography;
using System.Text;
using InforceTask.Application.Interfaces;
using InforceTask.Application.Models;

namespace InforceTask.Application.Services;

public class UrlShorteningService : IUrlShorteningService
{
    private readonly ApplicationDbContext _context;

    public UrlShorteningService(ApplicationDbContext context)
    {
        _context = context;
    }

    public string ShortenUrl(string originalUrl)
    {
        using (var sha256 = SHA256.Create())
        {
            var hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(originalUrl));
            var shortUrl = Convert.ToBase64String(hash).Substring(0, 8);
            return shortUrl;
        }
    }

    public Url CreateShortUrl(string originalUrl, string createdBy)
    {
        var existingUrl = _context.Urls.FirstOrDefault(u => u.OriginalUrl == originalUrl);
        if (existingUrl != null)
        {
            throw new Exception("URL already exists");
        }

        var shortUrl = ShortenUrl(originalUrl);
        var url = new Url
        {
            OriginalUrl = originalUrl,
            ShortUrl = shortUrl,
            CreatedBy = createdBy,
            CreatedDate = DateTime.Now
        };

        _context.Urls.Add(url);
        _context.SaveChanges();

        return url;
    }
}