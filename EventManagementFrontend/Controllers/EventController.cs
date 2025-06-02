using EventManagement.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using System;

namespace EventManagementFrontend.Controllers
{
    // Controller for handling user login, logout, and registration
    public class LoginController : Controller
    {
        private readonly HttpClient _httpClient;

        // Constructor with dependency injection for HttpClient
        public LoginController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new System.Uri("http://localhost:5199/"); // Adjust backend URL as needed
        }

        // GET: /Login/Index
        // Returns the login view
        [HttpGet]
        public IActionResult Index() => View();

        // POST: /Login/Index
        // Handles user login, calls backend Auth API, stores JWT and role in session
        [HttpPost]
        public async Task<IActionResult> Index(string emailId, string password)
        {
            var loginRequest = new { EmailId = emailId, Password = password };
            var json = JsonSerializer.Serialize(loginRequest);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Call backend Auth API to get JWT token
            var response = await _httpClient.PostAsync("api/Auth/login", content);
            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError("", "Invalid credentials");
                return View();
            }

            var responseJson = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(responseJson);
            var token = doc.RootElement.GetProperty("token").GetString();

            // Decode JWT to extract role claim
            var handler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(token);

            // Try multiple ways to get role claim for compatibility
            var role = jwt.Claims.FirstOrDefault(c => c.Type == "role")?.Value
                       ?? jwt.Claims.FirstOrDefault(c => c.Type.EndsWith("role"))?.Value
                       ?? "User";

            // Store JWT and user info in session
            HttpContext.Session.SetString("JWToken", token);
            HttpContext.Session.SetString("UserRole", role);
            HttpContext.Session.SetString("EmailId", emailId);

            // Redirect based on user role
            if (string.Equals(role, "Admin", StringComparison.OrdinalIgnoreCase))
                return RedirectToAction("Index", "Admin");
            else
                return RedirectToAction("Index", "Participant");
        }

        // GET: /Login/Logout
        // Clears session and logs out the user
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }

        // GET: /Login/Register
        // Returns the registration view
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
    }
}