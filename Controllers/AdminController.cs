using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using social_network.Data;
using social_network.Models;
using social_network.Entity;
using Microsoft.AspNetCore.Authorization;

namespace social_network.Controllers;

[Authorize(Roles = "admin")]
public class AdminController : Controller
{
    private readonly ILogger<AdminController> _logger;
    private readonly SocialNetworkContext _context;

    public AdminController(ILogger<AdminController> logger, SocialNetworkContext context)
    {
        _logger = logger;
        _context = context;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Place()
    {
        return View();
    }

    [HttpPost]
    public IActionResult PlaceRegister(Place place)
    {
        this._logger.LogInformation("place title" + place.title);

        this._context.Add(place);
        this._context.SaveChanges();
        return View("Place");
    }

    [HttpPost]
    public JsonResult getPlaces(Paginate paginate)
    {
        List<Place> places = this._context.Place.Skip(paginate.limit * (paginate.pageNumber - 1)).Take(paginate.limit).ToList();
        int totalRecord = this._context.Place.Count();
        PaginateResult<Place> paginateResult = new PaginateResult<Place> { data = places, totalRecords = totalRecord, limit = paginate.limit, currentPage = paginate.pageNumber };
        return Json(paginateResult);
    }

    [HttpPost]
    public IActionResult Update(int id, string title, string description, string countryCode)
    {
        var place = _context.Place.Find(id);

        if (place == null)
        {
            return NotFound();
        }

        place.title = title;
        place.description = description;
        place.countryCode = countryCode;

        _context.SaveChanges();
        return View("Place");
    }

    [HttpPost]
    public IActionResult Delete(int id)
    {
        var place = _context.Place.Find(id);
        if (place == null)
        {
            return NotFound();
        }

        _context.Place.Remove(place);
        _context.SaveChanges();

        return View("Place");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
