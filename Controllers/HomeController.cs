using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using social_network.Data;
using social_network.Entity;
using social_network.Models;
using System.Security.Claims;

namespace social_network.Controllers;

[Authorize]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly SocialNetworkContext _context;
    private IHttpContextAccessor _httpContextAccessor;
    private string userId;

    public HomeController(ILogger<HomeController> logger, SocialNetworkContext context, IHttpContextAccessor httpContextAccessor)
    {
        _logger = logger;
        _context = context;
        _httpContextAccessor = httpContextAccessor;
        userId = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Name).Value;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public async Task<JsonResult> getPosts(PaginateScroll paginateScroll)
    {
        List<PostResult> result = this._context.Post.Join(this._context.Media, post => post.id, media => media.postId, (post, media) => new
        {
            post = post,
            media = media
        }).GroupBy(p => p.post).Select(g => new
        {
            post = g.Key,
            media = g.Select(p => p.media).ToList()
        }).Join(this._context.User, com => com.post.userId, u => u.id, (com, u) => new
        {
            post = com.post,
            media = com.media,
            user = u
        })
        .Join(this._context.Place, com => com.post.placeId, p => p.id, (com, p) => new PostResult
        {
            post = com.post,
            media = com.media,
            owner = com.user,
            place = p
        }).Select(g=> new PostResult{
            post = g.post,
            media = g.media,
            owner = g.owner,
            place = g.place, 
        }).ToList();

        PaginateResult<PostResult> paginateResult = new PaginateResult<PostResult>() { data = result, limit = paginateScroll.limit };

        return Json(paginateResult);
    }

    [HttpGet]
    public JsonResult getAllPlaces()
    {
        List<Place> allPlaces = this._context.Place.ToList();

        PaginateResult<Place> result = new PaginateResult<Place> { data = allPlaces, limit = 0, currentPage = 1 };

        return Json(result);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
