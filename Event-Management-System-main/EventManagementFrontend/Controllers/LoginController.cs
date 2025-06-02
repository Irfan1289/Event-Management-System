using EventManagement.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net.Http.Headers;
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
            _httpClient.BaseAddress = new System.Uri("http://localhost:5199/");
        }
 
        [HttpGet]
        public IActionResult Index() => View();
 
        [HttpPost]
        public async Task<IActionResult> Index(string emailId, string password)
        {
            var loginModel = new LoginRequest { EmailId = emailId, Password = password };
            // Send login request to backend and get JWT token
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
 
            var json = JsonSerializer.Serialize(loginModel, options);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
 
            // Make the HTTP POST request to the login endpoint
            var response = await _httpClient.PostAsync("api/auth/login", content);
 
            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError("", "Invalid email or password");
                return View();
            }
 
            // Read the response content
            var responseBody = await response.Content.ReadAsStringAsync();
            // Deserialize the response to get the token
            var tokenResult = JsonSerializer.Deserialize<TokenResult>(responseBody, options);
            // Check if token is null or empty
            if (tokenResult == null || string.IsNullOrEmpty(tokenResult.Token))
            {
                ModelState.AddModelError("", "Token not received from server.");
                return View();
            }
 
            // Extract token from response
            var token = tokenResult.Token;
 
            // Store token and emailId in session
            HttpContext.Session.SetString("JWToken", token);
            HttpContext.Session.SetString("EmailId", emailId);
 
            // Set token in header for next request
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var userResponse = await _httpClient.GetAsync($"api/userinfo/{emailId}");
 
            if (userResponse.IsSuccessStatusCode)
            {
                var userJson = await userResponse.Content.ReadAsStringAsync();
                var user = JsonSerializer.Deserialize<UserInfo>(userJson, options);
 
                if (user != null)
                {
                    HttpContext.Session.SetString("UserRole", user.Role);
 
                    if (user.Role == "Admin")
                        return RedirectToAction("Index", "Admin");
                    else
                        return RedirectToAction("Index", "Participant");
                }
            }
 
            // If user not found or role not recognized, redirect to participant index
            return RedirectToAction("Index", "Participant");
        }
 
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }
 
        // Register action methods
        [HttpGet]
        public IActionResult Register() => View();
 
        [HttpPost]
        public async Task<IActionResult> Register(UserInfo user)
        {
            if (!ModelState.IsValid)
            {
                return View(user);
            }
 
            // Serialize user object to JSON
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
 
            var json = JsonSerializer.Serialize(user, options);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
 
            var response = await _httpClient.PostAsync("api/userinfo", content);
 
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
 
            ModelState.AddModelError("", "Registration failed. Try again.");
            return View(user);
        }
 
        // Classes for deserialization
        private class TokenResult
        {
            public string? Token { get; set; }
        }
 
        // Class for login request
        private class LoginRequest
        {
            public string EmailId { get; set; }
            public string Password { get; set; }
        }
    }
}
 
 