using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using social_network.Data;
using social_network.Entity;
using social_network.Models;

namespace social_network.Controllers;

public class MediaController : Controller
{
    private readonly IConfiguration _config;
    private readonly ILogger<MediaController> _logger;
    private readonly SocialNetworkContext _context;

    public MediaController(ILogger<MediaController> logger, SocialNetworkContext context, IConfiguration config)
    {
        _logger = logger;
        _context = context;
        _config = config;
    }

    public bool Index()
    {
        return true;
    }



    public async Task<ActionResult> upload(IFormFile file)
    {
        return Ok("haha");
    }

    [HttpPost]
    public async Task<ActionResult> uploads(List<IFormFile> files)
    {
        try
        {
            long size = files.Sum(f => f.Length);

            List<string> pathFiles = new List<string>();

            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    var filePath = Path.Combine(_config["StoredFilesPath"],Path.GetFileName(formFile.FileName));

                    if (!Directory.Exists(Path.Combine(_config["StoredFilesPath"])))
                    {
                        Directory.CreateDirectory(Path.Combine(_config["StoredFilesPath"]));
                    }
                    pathFiles.Add(filePath);

                    using (var stream = System.IO.File.Create(filePath))
                    {
                        this._logger.LogInformation("in here uploading" + filePath);
                        formFile.CopyTo(stream);
                    }
                }
            }

            // Process uploaded files
            // Don't rely on or trust the FileName property without validation.

            return Ok(pathFiles);
        }
        catch (Exception e)
        {
            return BadRequest(e);
            //  Block of code to handle errors
        }

    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
