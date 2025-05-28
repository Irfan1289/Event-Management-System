using EventManagement.Model;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text.Json;
using System.Text;
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

        public async Task<IActionResult> Index()
        {
            var response = await _httpClient.GetAsync("api/EventDetails");
            if (!response.IsSuccessStatusCode) return View(new List<EventDetails>());

            var eventsJson = await response.Content.ReadAsStringAsync();
            var events = JsonSerializer.Deserialize<List<EventDetails>>(eventsJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return View(events);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(EventDetails eventDetails)
        {
            var json = JsonSerializer.Serialize(eventDetails);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("api/EventDetails", content);

            if (response.IsSuccessStatusCode)
                return RedirectToAction("Index");

            ModelState.AddModelError("", "Error creating event");
            return View(eventDetails);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var response = await _httpClient.GetAsync($"api/EventDetails/{id}");
            if (!response.IsSuccessStatusCode) return NotFound();

            var json = await response.Content.ReadAsStringAsync();
            var eventDetails = JsonSerializer.Deserialize<EventDetails>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return View("EditEvent", eventDetails);
        }

        [HttpPost]
        public async Task<IActionResult> EditEvent(EventDetails eventDetails)
        {
            var json = JsonSerializer.Serialize(eventDetails);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"api/EventDetails/{eventDetails.EventId}", content);

            if (response.IsSuccessStatusCode)
                return RedirectToAction("Index");

            ModelState.AddModelError("", "Error updating event");
            return View("EditEvent", eventDetails);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _httpClient.GetAsync($"api/EventDetails/{id}");
            if (!response.IsSuccessStatusCode) return NotFound();

            var json = await response.Content.ReadAsStringAsync();
            var eventDetails = JsonSerializer.Deserialize<EventDetails>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return View(eventDetails);
        }

        [HttpPost]
public async Task<IActionResult> DeleteConfirmed(int id)
{
    var response = await _httpClient.DeleteAsync($"api/EventDetails/{id}");
    if (response.IsSuccessStatusCode)
        return RedirectToAction("Index");

    ModelState.AddModelError("", "Error deleting event");
    return RedirectToAction("Index");
}

    }
}
