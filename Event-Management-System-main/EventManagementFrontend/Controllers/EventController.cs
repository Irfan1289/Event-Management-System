using EventManagement.Model;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace EventManagementFrontend.Controllers
{
    public class EventController : Controller
    {
        private readonly HttpClient _httpClient;

        public EventController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new System.Uri("http://localhost:5199/");
        }

        // GET: /Event/Index
        public async Task<IActionResult> Index()
        {
            var response = await _httpClient.GetAsync("api/EventDetails");
            if (!response.IsSuccessStatusCode)
                return View(new List<EventDetails>());

            var json = await response.Content.ReadAsStringAsync();
            var events = JsonSerializer.Deserialize<List<EventDetails>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return View(events);
        }

        // GET: /Event/Details/{id}
        public async Task<IActionResult> Details(int id)
        {
            var response = await _httpClient.GetAsync($"api/EventDetails/{id}");
            if (!response.IsSuccessStatusCode)
                return NotFound();

            var json = await response.Content.ReadAsStringAsync();
            var evt = JsonSerializer.Deserialize<EventDetails>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return View(evt);
        }
    }
}