using EventManagement.Model;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text.Json;
using System.Text;
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

        public async Task<IActionResult> Index()
        {
            var response = await _httpClient.GetAsync("api/SessionInfo");
            if (!response.IsSuccessStatusCode) return View(new List<SessionInfo>());

            var sessionsJson = await response.Content.ReadAsStringAsync();
            var sessions = JsonSerializer.Deserialize<List<SessionInfo>>(sessionsJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return View(sessions);
        }

        // Add Create/Edit/Delete similarly
        public async Task<IActionResult> Create(SessionInfo sessionInfo)
        {
            var json = JsonSerializer.Serialize(sessionInfo);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("api/SessionInfo", content);

            if (response.IsSuccessStatusCode)
                return RedirectToAction("Index");

            ModelState.AddModelError("", "Error creating session");
            return View(sessionInfo);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var response = await _httpClient.GetAsync($"api/SessionInfo/{id}");
            if (!response.IsSuccessStatusCode) return NotFound();

            var json = await response.Content.ReadAsStringAsync();
            var sessionInfo = JsonSerializer.Deserialize<SessionInfo>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return View(sessionInfo);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/SessionInfo/{id}");
            if (!response.IsSuccessStatusCode) return NotFound();

            return RedirectToAction("Index");
        }
    }
}