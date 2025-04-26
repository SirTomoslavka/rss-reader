using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using rss_reader.Data;
using rss_reader.Models;
using rss_reader.Services;

namespace rss_reader.Controllers;

public class FeedController(RssService rss) : Controller
{
    [HttpGet("/feed")]
    public async Task<IActionResult> Index(Guid? feedId, DateTime? fromDate, DateTime? toDate)
    {
        if (!feedId.HasValue)
        {
            return BadRequest();
        }
        
        Feed? feed;
        
        if (fromDate.HasValue && toDate.HasValue)
        {
            feed = await rss.GetFeedWithArticlesAsync(feedId.Value, fromDate.Value, toDate.Value);
        }
        else
        {
            feed = await rss.GetFeedWithArticlesAsync(feedId.Value);
        }


        if (feed == null)
        {
            return NotFound();
        }

        return View(feed);
    }
    
    [HttpGet("/feed/new")]
    public IActionResult FeedForm()
    {
        return View();
    }

    [HttpGet("/feed/detail")]
    public async Task<IActionResult> Feed(Guid feedId)
    {
        var feed = await rss.GetFeedAsync(feedId);

        if (feed == null)
        {
            return NotFound();
        }

        return View(feed);
    }
    
    [HttpPost("/feed/refresh")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RefreshArticles(Guid feedId)
    {
        await rss.ReloadNewArticlesAsync(feedId);
        
        return RedirectToAction(nameof(Index), new { feedId });
    }

    
    [HttpGet("/feed/articles/search")]
    public IActionResult Search(Guid id)
    {
        var url = rss.GetArticleUrl(id);

        if (string.IsNullOrWhiteSpace(url))
        {
            return NotFound();
        }

        return Redirect(url);
    }
    
    [HttpPost("/feed/add")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddFeedPost(CreateFeedModel model)
    {
        if (!ModelState.IsValid)
        {
            return RedirectToAction("Error", "Feed");
        }
        
        try
        {
            var feed = await rss.AddFeedAndFetchArticlesAsync(model.Url, model.Name);
            return RedirectToAction(nameof(Index), new { feedId = feed.FeedId });
        }
        catch (Exception)
        {
            return RedirectToAction("Error", "Home");
        }
    }
    
    [HttpDelete("feed/delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(string feedsId)
    {
        await rss.DeleteFeedsAsync(feedsId);
        
        return RedirectToAction("Index", "Home");
    }
    
    [HttpGet("feed/error")]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}