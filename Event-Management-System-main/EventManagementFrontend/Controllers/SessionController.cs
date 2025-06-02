using EventManagement.Model;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace EventManagementFrontend.Controllers
{
    public class SessionController : Controller
    {
        private readonly HttpClient _httpClient;

        public SessionController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new System.Uri("http://localhost:5199/");
        }

        // GET: /Session/Index
        public async Task<IActionResult> Index()
        {
            var response = await _httpClient.GetAsync("api/SessionInfo");
            if (!response.IsSuccessStatusCode)
                return View(new List<SessionInfo>());

            var json = await response.Content.ReadAsStringAsync();
            var sessions = JsonSerializer.Deserialize<List<SessionInfo>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return View(sessions);
        }

        // GET: /Session/Details/{id}
        public async Task<IActionResult> Details(int id)
        {
            var response = await _httpClient.GetAsync($"api/SessionInfo/{id}");
            if (!response.IsSuccessStatusCode)
                return NotFound();

            var json = await response.Content.ReadAsStringAsync();
            var session = JsonSerializer.Deserialize<SessionInfo>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return View(session);
        }
    }
}