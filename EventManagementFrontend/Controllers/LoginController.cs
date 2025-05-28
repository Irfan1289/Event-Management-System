using EventManagement.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EventManagementFrontend.Controllers
{
    public class LoginController : Controller
    {
        private readonly HttpClient _httpClient;

        public LoginController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new System.Uri("http://localhost:5199/"); // Adjust backend URL
        }

        [HttpGet]
        public IActionResult Index() => View();

        [HttpPost]
        public async Task<IActionResult> Index(string emailId, string password)
        {
            var response = await _httpClient.GetAsync($"api/userinfo/{emailId}");
            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError("", "Invalid user");
                return View();
            }

            var userJson = await response.Content.ReadAsStringAsync();
            var user = JsonSerializer.Deserialize<UserInfo>(userJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (user == null || user.Password != password)
            {
                ModelState.AddModelError("", "Invalid password");
                return View();
            }

            HttpContext.Session.SetString("UserRole", user.Role);
            HttpContext.Session.SetString("EmailId", user.EmailId);

            if (user.Role == "Admin")
                return RedirectToAction("Index", "Admin");
            else
                return RedirectToAction("Index", "Participant");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }

        // GET: Register new user
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // POST: Register new user
        [HttpPost]
        public async Task<IActionResult> Register(UserInfo user)
        {
            if (!ModelState.IsValid)
            {
                return View(user);
            }

            var json = JsonSerializer.Serialize(user);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("api/userinfo", content);

            if (response.IsSuccessStatusCode)
            {
                // After successful registration, redirect to Login page
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Registration failed. Try again.");
            return View(user);
        }
    }
}
