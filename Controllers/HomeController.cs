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
        List<PostResult> result = this._context.Post.Where(p => p.id > paginateScroll.startId).Take(paginateScroll.limit).GroupJoin(this._context.Media, p => p.id, me => me.postId, (p, meg) => new
        {
            post = p,
            media = meg.ToList()
        }).ToList().GroupJoin(this._context.Reaction, pm => pm.post.id, rec => rec.postId, (pm, rec) => new
        {
            post = pm.post,
            media = pm.media,
            reaction = rec.ToList()
        }).ToList().Join(this._context.User, g => g.post.userId, u => u.id, (g, u) => new
        {
            post = g.post,
            media = g.media,
            reaction = g.reaction,
            owner = u
        }).Join(this._context.Place, g => g.post.placeId, pl => pl.id, (g, pl) => new PostResult
        {
            post = g.post,
            media = g.media,
            reaction = g.reaction,
            owner = g.owner,
            place = pl
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

    [HttpPost("/createpost")]
    public async Task<ActionResult> CreatePost(Post post, List<string> mediaUrls)
    {
        post.userId = Int32.Parse(userId);

        await this._context.Post.AddAsync(post);
        this._context.SaveChanges();
        if (mediaUrls.Any())
        {
            foreach (string item in mediaUrls)
            {
                this._logger.LogInformation("post id" + post.id);
                Media media = new Media() { userId = Int32.Parse(userId), postId = post.id, type = MediaTypeEnum.AVATAR, url = item };
                this._context.AddAsync(media);
            }
        }

        int saved = await this._context.SaveChangesAsync();

        return Ok(saved);
    }

    [HttpPost("react")]
    public async Task<JsonResult> reactPost(int postId, ReactionEnum react)
    {

        bool isExist = this._context.Reaction.Any(rec => rec.postId == postId && rec.userId == Int32.Parse(userId) && rec.reaction == ReactionEnum.LIKE);
        bool isOppositeReact = false;

        if (isExist)
        {
            var userToRemove = this._context.Reaction.SingleOrDefault(x => x.postId == postId && x.userId == Int32.Parse(userId) && x.reaction == ReactionEnum.LIKE);
            if (userToRemove != null)
            {
                this._context.Reaction.Remove(userToRemove);
                isOppositeReact = true;
            }
        }
        else
        {
            await this._context.Reaction.AddAsync(new Reaction { userId = Int32.Parse(userId), postId = postId, reaction = ReactionEnum.LIKE });

        }

        int recordEffected = await this._context.SaveChangesAsync();
        int currentQuantity = this._context.Reaction.Count(r => r.postId == postId && r.userId == Int32.Parse(userId) && r.reaction == react);
        return Json(new ReactionResult { postId = postId, effected = recordEffected, react = react, oppositeReact = isOppositeReact, currentQuantity = currentQuantity });
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
