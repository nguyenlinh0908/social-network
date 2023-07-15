using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using social_network.Data;
using social_network.Models;
using System.Security.Cryptography;
using System.Text;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

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

    [HttpPost("login")]
    public async Task<IActionResult> Login(string phoneOrEmail, string password)
    {
        User verifyUser = this.verifyUser(phoneOrEmail, password);

        if (verifyUser == null)
        {
            ViewBag["Message"] = "Thông tin đăng nhập không chính xác";
            return RedirectToAction(null);
        }

        var claims = new List<Claim>{
            new Claim(ClaimTypes.Name,  verifyUser.id.ToString()),
            new Claim("email", verifyUser.email),
            new Claim("phone", verifyUser.phoneNumber),
            new Claim(ClaimTypes.Role, verifyUser.role.ToString()),
        };

        var claimsIdentity = new ClaimsIdentity(
                    claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var authProperties = new AuthenticationProperties
        {
        };

        await HttpContext.SignInAsync(
           CookieAuthenticationDefaults.AuthenticationScheme,
           new ClaimsPrincipal(claimsIdentity),
           authProperties);

        this._logger.LogInformation("User {Id} logged in at {Time}.",
                   verifyUser.id, DateTime.UtcNow);
                   
        return RedirectToAction("Index", "Home", new { area = "" });
    }

    private User verifyUser(string phoneOrEmail, string password)
    {
        User user = this._context.User.Where(user => user.email == phoneOrEmail || user.phoneNumber == phoneOrEmail).FirstOrDefault();

        if (user == null)
        {
            return null;
        }


        string hashedLoginPassword = this.hashPassword(password);
        bool isValidPassword = this.isComparePassword(hashedLoginPassword, user.password);

        if (!isValidPassword)
        {
            return null;
        }

        return user;
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
