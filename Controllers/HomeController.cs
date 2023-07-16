using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using social_network.Data;
using social_network.Entity;
using social_network.Models;

namespace social_network.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly SocialNetworkContext _context;

    public HomeController(ILogger<HomeController> logger, SocialNetworkContext context)
    {
        _logger = logger;
        _context = context;
    }

    public IActionResult Index()
    {
        return View();
    }


    [HttpPost]
    public bool CreatePost(Post post)
    {
        
        return true;
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
