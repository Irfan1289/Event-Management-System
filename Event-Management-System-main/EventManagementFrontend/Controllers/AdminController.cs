using EventManagement.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Net.Http;
using System.Text.Json;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Net.Http.Headers;
 
namespace EventManagementFrontend.Controllers
{
    public class AdminController : BaseController
    {
 
        private readonly IHttpClientFactory _httpClientFactory;
 
        // Constructor to inject IHttpClientFactory
        public AdminController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
 
        // Check if the user is an admin
        private bool IsAdmin()
        {
            return HttpContext.Session.GetString("UserRole") == "Admin";
        }
 
        // Get HttpClient with JWT token from session
        private HttpClient GetHttpClientWithToken()
        {
            var token = HttpContext.Session.GetString("JWToken");
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new System.Uri("http://localhost:5199/");
            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            return client;
        }
 
        // Dashboard showing Events, Sessions, Speakers
        public async Task<IActionResult> Index()
        {
            if (!IsAdmin()) return RedirectToAction("Index", "Login");
 
            // Create HttpClient with JWT token
            var client = GetHttpClientWithToken();
 
            // Events
            var events = new List<EventDetails>();
            var eventsResponse = await client.GetAsync("api/EventDetails");
            if (eventsResponse.IsSuccessStatusCode)
            {
                var json = await eventsResponse.Content.ReadAsStringAsync();
                events = JsonSerializer.Deserialize<List<EventDetails>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
 
            // Sessions
            var sessions = new List<SessionInfo>();
            var sessionsResponse = await client.GetAsync("api/SessionInfo");
            if (sessionsResponse.IsSuccessStatusCode)
            {
                var json = await sessionsResponse.Content.ReadAsStringAsync();
                sessions = JsonSerializer.Deserialize<List<SessionInfo>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
 
            // Speakers
            var speakers = new List<SpeakersDetails>();
            var speakersResponse = await client.GetAsync("api/SpeakersDetails");
            if (speakersResponse.IsSuccessStatusCode)
            {
                var json = await speakersResponse.Content.ReadAsStringAsync();
                speakers = JsonSerializer.Deserialize<List<SpeakersDetails>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
 
            ViewBag.Sessions = sessions;
            ViewBag.Speakers = speakers;
            return View(events);
        }
 
        #region Event CRUD
        [HttpGet]
        public IActionResult CreateEvent()
        {
            if (!IsAdmin()) return RedirectToAction("Index", "Login");
            return View();
        }
 
        [HttpPost]
        public async Task<IActionResult> CreateEvent(EventDetails eventDetails)
        {
            if (!IsAdmin()) return RedirectToAction("Index", "Login");
 
            var client = GetHttpClientWithToken();
            var json = JsonSerializer.Serialize(eventDetails);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
 
            var response = await client.PostAsync("api/EventDetails", content);
            if (response.IsSuccessStatusCode) return RedirectToAction("Index");
 
            ModelState.AddModelError("", "Error creating event");
            return View(eventDetails);
        }
 
        [HttpGet]
        public async Task<IActionResult> EditEvent(int id)
        {
            if (!IsAdmin()) return RedirectToAction("Index", "Login");
 
            var client = GetHttpClientWithToken();
            var response = await client.GetAsync($"api/EventDetails/{id}");
            if (!response.IsSuccessStatusCode) return NotFound();
 
            var json = await response.Content.ReadAsStringAsync();
            var eventDetails = JsonSerializer.Deserialize<EventDetails>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return View(eventDetails);
        }
 
        [HttpPost]
        public async Task<IActionResult> EditEvent(EventDetails eventDetails)
        {
            if (!IsAdmin()) return RedirectToAction("Index", "Login");
 
            var client = GetHttpClientWithToken();
            var json = JsonSerializer.Serialize(eventDetails);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
 
            var response = await client.PutAsync($"api/EventDetails/{eventDetails.EventId}", content);
            if (response.IsSuccessStatusCode) return RedirectToAction("Index");
 
            ModelState.AddModelError("", "Error updating event");
            return View(eventDetails);
        }
 
        [HttpGet]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            if (!IsAdmin()) return RedirectToAction("Index", "Login");
 
            var client = GetHttpClientWithToken();
            var response = await client.GetAsync($"api/EventDetails/{id}");
            if (!response.IsSuccessStatusCode) return NotFound();
 
            var json = await response.Content.ReadAsStringAsync();
            var eventDetails = JsonSerializer.Deserialize<EventDetails>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return View(eventDetails);
        }
 
        [HttpPost]
        public async Task<IActionResult> DeleteEvent(EventDetails eventDetails)
        {
            if (!IsAdmin()) return RedirectToAction("Index", "Login");
 
            var client = GetHttpClientWithToken();
            var response = await client.DeleteAsync($"api/EventDetails/{eventDetails.EventId}");
            if (response.IsSuccessStatusCode) return RedirectToAction("Index");
 
            ModelState.AddModelError("", "Error deleting event");
            return View(eventDetails);
        }
        #endregion
 
        #region Session CRUD
        [HttpGet]
        public IActionResult CreateSession()
        {
            if (!IsAdmin()) return RedirectToAction("Index", "Login");
            return View();
        }
 
        [HttpPost]
        public async Task<IActionResult> CreateSession(SessionInfo sessionInfo)
        {
            if (!IsAdmin()) return RedirectToAction("Index", "Login");
 
            var client = GetHttpClientWithToken();
            var json = JsonSerializer.Serialize(sessionInfo);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
 
            var response = await client.PostAsync("api/SessionInfo", content);
            if (response.IsSuccessStatusCode) return RedirectToAction("Index");
 
            ModelState.AddModelError("", "Error creating session");
            return View(sessionInfo);
        }
 
        [HttpGet]
        public async Task<IActionResult> EditSession(int id)
        {
            if (!IsAdmin()) return RedirectToAction("Index", "Login");
 
            var client = GetHttpClientWithToken();
            var response = await client.GetAsync($"api/SessionInfo/{id}");
            if (!response.IsSuccessStatusCode) return NotFound();
 
            var json = await response.Content.ReadAsStringAsync();
            var sessionInfo = JsonSerializer.Deserialize<SessionInfo>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return View(sessionInfo);
        }
 
        [HttpPost]
        public async Task<IActionResult> EditSession(SessionInfo sessionInfo)
        {
            if (!IsAdmin()) return RedirectToAction("Index", "Login");
 
            var client = GetHttpClientWithToken();
            var json = JsonSerializer.Serialize(sessionInfo);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
 
            var response = await client.PutAsync($"api/SessionInfo/{sessionInfo.SessionId}", content);
            if (response.IsSuccessStatusCode) return RedirectToAction("Index");
 
            ModelState.AddModelError("", "Error updating session");
            return View(sessionInfo);
        }
 
        [HttpGet]
        public async Task<IActionResult> DeleteSession(int id)
        {
            if (!IsAdmin()) return RedirectToAction("Index", "Login");
 
            var client = GetHttpClientWithToken();
            var response = await client.GetAsync($"api/SessionInfo/{id}");
            if (!response.IsSuccessStatusCode) return NotFound();
 
            var json = await response.Content.ReadAsStringAsync();
            var sessionInfo = JsonSerializer.Deserialize<SessionInfo>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return View(sessionInfo);
        }
 
        [HttpPost]
        public async Task<IActionResult> DeleteSession(SessionInfo sessionInfo)
        {
            if (!IsAdmin()) return RedirectToAction("Index", "Login");
 
            var client = GetHttpClientWithToken();
            var response = await client.DeleteAsync($"api/SessionInfo/{sessionInfo.SessionId}");
            if (response.IsSuccessStatusCode) return RedirectToAction("Index");
 
            ModelState.AddModelError("", "Error deleting session");
            return View(sessionInfo);
        }
        #endregion
 
        #region Speakers CRUD
        public async Task<IActionResult> Speakers()
        {
            if (!IsAdmin()) return RedirectToAction("Index", "Login");
 
            var client = GetHttpClientWithToken();
            var speakers = new List<SpeakersDetails>();
            var response = await client.GetAsync("api/SpeakersDetails");
 
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                speakers = JsonSerializer.Deserialize<List<SpeakersDetails>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
 
            return View(speakers);
        }
 
        [HttpGet]
        public IActionResult CreateSpeaker()
        {
            if (!IsAdmin()) return RedirectToAction("Index", "Login");
            return View();
        }
 
        [HttpPost]
        public async Task<IActionResult> CreateSpeaker(SpeakersDetails speaker)
        {
            if (!IsAdmin()) return RedirectToAction("Index", "Login");
 
            var client = GetHttpClientWithToken();
            var json = JsonSerializer.Serialize(speaker);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
 
            var response = await client.PostAsync("api/SpeakersDetails", content);
            if (response.IsSuccessStatusCode) return RedirectToAction("Speakers");
 
            ModelState.AddModelError("", "Error creating speaker");
            return View(speaker);
        }
 
        [HttpGet]
        public async Task<IActionResult> EditSpeaker(int id)
        {
            if (!IsAdmin()) return RedirectToAction("Index", "Login");
 
            var client = GetHttpClientWithToken();
            var response = await client.GetAsync($"api/SpeakersDetails/{id}");
            if (!response.IsSuccessStatusCode) return NotFound();
 
            var json = await response.Content.ReadAsStringAsync();
            var speaker = JsonSerializer.Deserialize<SpeakersDetails>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return View(speaker);
        }
 
        [HttpPost]
        public async Task<IActionResult> EditSpeaker(SpeakersDetails speaker)
        {
            if (!IsAdmin()) return RedirectToAction("Index", "Login");
 
            var client = GetHttpClientWithToken();
            var json = JsonSerializer.Serialize(speaker);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
 
            var response = await client.PutAsync($"api/SpeakersDetails/{speaker.SpeakerId}", content);
            if (response.IsSuccessStatusCode) return RedirectToAction("Speakers");
 
            ModelState.AddModelError("", "Error updating speaker");
            return View(speaker);
        }
 
        [HttpGet]
        public async Task<IActionResult> DeleteSpeaker(int id)
        {
            if (!IsAdmin()) return RedirectToAction("Index", "Login");
 
            var client = GetHttpClientWithToken();
            var response = await client.GetAsync($"api/SpeakersDetails/{id}");
            if (!response.IsSuccessStatusCode) return NotFound();
 
            var json = await response.Content.ReadAsStringAsync();
            var speaker = JsonSerializer.Deserialize<SpeakersDetails>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return View(speaker);
        }
 
        [HttpPost]
        public async Task<IActionResult> DeleteSpeaker(SpeakersDetails speaker)
        {
            if (!IsAdmin()) return RedirectToAction("Index", "Login");
 
            var client = GetHttpClientWithToken();
            var response = await client.DeleteAsync($"api/SpeakersDetails/{speaker.SpeakerId}");
            if (response.IsSuccessStatusCode) return RedirectToAction("Speakers");
 
            ModelState.AddModelError("", "Error deleting speaker");
            return View(speaker);
        }
        #endregion
    }
}
 
 