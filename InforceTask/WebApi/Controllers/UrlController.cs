using InforceTask.Application.Models;
using InforceTask.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace InforceTask.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UrlController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<User> _userManager;
    private readonly UrlShorteningService _urlShorteningService;

    public UrlController(ApplicationDbContext context, UserManager<User>? userManager, UrlShorteningService urlShorteningService)
    {
        _context = context;
        _userManager = _userManager;
        _urlShorteningService = urlShorteningService;
    }

    [HttpGet]
    public IActionResult GetUrls()
    {
        var urls = _context.Urls.ToList();
        return Ok(urls);
    }

    [HttpPost]
    [Authorize]
    public IActionResult CreateUrl([FromBody] string originalUrl)
    {
        var createdBy = User.Identity.Name;
        try
        {
            var url = _urlShorteningService.CreateShortUrl(originalUrl, createdBy);
            return Ok(url);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    [Authorize]
    public IActionResult DeleteUrl(int id)
    {
        var url = _context.Urls.Find(id);
        if (url == null)
        {
            return NotFound();
        }

        if (url.CreatedBy != User.Identity.Name && !User.IsInRole("Admin"))
        {
            return Forbid();
        }

        _context.Urls.Remove(url);
        _context.SaveChanges();

        return NoContent();
    }
}