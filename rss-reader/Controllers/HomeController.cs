using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using rss_reader.Models;
using rss_reader.Services;

namespace rss_reader.Controllers;

public class HomeController(RssService rss) : Controller
{
    [HttpGet("")]
    public async Task<IActionResult> Index()
    {
        var model = await rss.GetAllFeedsAsync();

        return View(model);
    }

    [HttpGet("feed/search")]
    public IActionResult Search(Guid id)
    {
        return RedirectToAction(nameof(Index), "Feed", new { feedId = id });
    }

    [HttpGet("error")]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}