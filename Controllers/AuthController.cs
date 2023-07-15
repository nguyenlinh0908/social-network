using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using social_network.Data;
using social_network.Models;
using System.Security.Cryptography;
using System.Text;
namespace social_network.Controllers;

public class AuthController : Controller
{
    private readonly ILogger<AuthController> _logger;
    private readonly SocialNetworkContext _context;
    private readonly IConfiguration _config;


    public AuthController(ILogger<AuthController> logger, SocialNetworkContext _context, IConfiguration config)
    {
        this._logger = logger;
        this._context = _context;
        this._config = config;
    }

    [HttpGet("login")]
    public IActionResult Index()
    {
        return View("Login");
    }

    [HttpGet("register")]
    public IActionResult Register()
    {
        return View("Register");
    }

    [HttpPost("register")]
    public IActionResult RegisterAccount(User userInput)
    {
        if (String.IsNullOrEmpty(userInput.email) && String.IsNullOrEmpty(userInput.phoneNumber))
        {
            ViewData["Message"] = "Email hoặc số điện thoại không được để trống";

            return RedirectToAction("register");
        }

        if (String.IsNullOrEmpty(userInput.password))
        {
            ViewData["Message"] = "Mật khẩu không được để trống";
            return RedirectToAction(null);
        }

        bool isEmail = this.isValidEmail(userInput.email);

        if (!isEmail && String.IsNullOrEmpty(userInput.email))
        {
            ViewData["Message"] = "Email không được để trống";

            return RedirectToAction(null);
        }

        bool existEmail = this.isExistEmail(userInput.email);
        if (userInput.email != "" && !existEmail)
        {
            ViewData["Message"] = "Email đã tồn tại";
            return RedirectToAction(null);
        }

        bool existPhoneNumer = this.isExistPhoneNumer(userInput.phoneNumber);
        if (userInput.phoneNumber != "" && !existPhoneNumer)
        {
            ViewData["Message"] = "Số điện thoại đã tồn tại";
            return RedirectToAction(null);
        }

        userInput.password = this.hashPassword(userInput.password);

        this._context.User.Add(userInput);
        this._context.SaveChanges();

        // ViewData["message"] = "Tạo tài khoản thành công";
        return RedirectToAction("Index");
    }

    private string hashPassword(string password)
    {
        using (SHA256 sha256Hash = SHA256.Create())
        {
            // ComputeHash - returns byte array
            byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));

            // Convert byte array to a string
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                builder.Append(bytes[i].ToString("x2"));
            }
            return builder.ToString();
        }
    }

    private bool isComparePassword(string password1, string password2)
    {
        if (!String.Equals(password1, password2))
        {
            return false;
        }
        return true;
    }

    private bool isValidEmail(string email)
    {
        var trimmedEmail = email.Trim();

        if (trimmedEmail.EndsWith("."))
        {
            return false; // suggested by @TK-421
        }
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == trimmedEmail;
        }
        catch
        {
            return false;
        }
    }

    public bool isExistEmail(string email)
    {
        User user = this._context.User.Where(user => user.email == email).FirstOrDefault();

        this._logger.LogInformation("userid email", user);

        if (user != null)
        {
            return false;
        }
        return true;
    }

    public bool isExistPhoneNumer(string phoneNumber)
    {
        User user = this._context.User.Where(user => user.phoneNumber == phoneNumber).FirstOrDefault();
        if (user != null)
        {
            return false;
        }
        return true;
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
