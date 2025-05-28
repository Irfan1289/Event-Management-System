using EventManagement.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Net.Http;
using System.Text.Json;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace EventManagementFrontend.Controllers
{
    public class AdminController : BaseController
    {
        private readonly HttpClient _httpClient;

        public AdminController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new System.Uri("http://localhost:5199/");
        }

        private bool IsAdmin()
        {
            return HttpContext.Session.GetString("UserRole") == "Admin";
        }

        // Dashboard showing both Events and Sessions
public async Task<IActionResult> Index()
{
    if (!IsAdmin()) return RedirectToAction("Index", "Login");

    // Get events
    var eventsResponse = await _httpClient.GetAsync("api/EventDetails");
    var events = new List<EventDetails>();
    if (eventsResponse.IsSuccessStatusCode)
    {
        var json = await eventsResponse.Content.ReadAsStringAsync();
        events = JsonSerializer.Deserialize<List<EventDetails>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
    }

    // Get sessions
    var sessionsResponse = await _httpClient.GetAsync("api/SessionInfo");
    var sessions = new List<SessionInfo>();
    if (sessionsResponse.IsSuccessStatusCode)
    {
        var sessionJson = await sessionsResponse.Content.ReadAsStringAsync();
        sessions = JsonSerializer.Deserialize<List<SessionInfo>>(sessionJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
    }

    // Get speakers (added this part)
    var speakersResponse = await _httpClient.GetAsync("api/SpeakersDetails");
    var speakers = new List<SpeakersDetails>();
    if (speakersResponse.IsSuccessStatusCode)
    {
        var speakerJson = await speakersResponse.Content.ReadAsStringAsync();
        speakers = JsonSerializer.Deserialize<List<SpeakersDetails>>(speakerJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
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

            var json = JsonSerializer.Serialize(eventDetails);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("api/EventDetails", content);
            if (response.IsSuccessStatusCode) return RedirectToAction("Index");

            ModelState.AddModelError("", "Error creating event");
            return View(eventDetails);
        }

        [HttpGet]
        public async Task<IActionResult> EditEvent(int id)
        {
            if (!IsAdmin()) return RedirectToAction("Index", "Login");

            var response = await _httpClient.GetAsync($"api/EventDetails/{id}");
            if (!response.IsSuccessStatusCode) return NotFound();

            var json = await response.Content.ReadAsStringAsync();
            var eventDetails = JsonSerializer.Deserialize<EventDetails>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return View(eventDetails);
        }

        [HttpPost]
        public async Task<IActionResult> EditEvent(EventDetails eventDetails)
        {
            if (!IsAdmin()) return RedirectToAction("Index", "Login");

            var json = JsonSerializer.Serialize(eventDetails);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"api/EventDetails/{eventDetails.EventId}", content);
            if (response.IsSuccessStatusCode) return RedirectToAction("Index");

            ModelState.AddModelError("", "Error updating event");
            return View(eventDetails);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            if (!IsAdmin()) return RedirectToAction("Index", "Login");

            var response = await _httpClient.GetAsync($"api/EventDetails/{id}");
            if (!response.IsSuccessStatusCode) return NotFound();

            var json = await response.Content.ReadAsStringAsync();
            var eventDetails = JsonSerializer.Deserialize<EventDetails>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return View(eventDetails);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteEvent(EventDetails eventDetails)
        {
            if (!IsAdmin()) return RedirectToAction("Index", "Login");

            var response = await _httpClient.DeleteAsync($"api/EventDetails/{eventDetails.EventId}");
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

            var json = JsonSerializer.Serialize(sessionInfo);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("api/SessionInfo", content);
            if (response.IsSuccessStatusCode) return RedirectToAction("Index");

            ModelState.AddModelError("", "Error creating session");
            return View(sessionInfo);
        }

        [HttpGet]
        public async Task<IActionResult> EditSession(int id)
        {
            if (!IsAdmin()) return RedirectToAction("Index", "Login");

            var response = await _httpClient.GetAsync($"api/SessionInfo/{id}");
            if (!response.IsSuccessStatusCode) return NotFound();

            var json = await response.Content.ReadAsStringAsync();
            var sessionInfo = JsonSerializer.Deserialize<SessionInfo>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return View(sessionInfo);
        }

        [HttpPost]
        public async Task<IActionResult> EditSession(SessionInfo sessionInfo)
        {
            if (!IsAdmin()) return RedirectToAction("Index", "Login");

            var json = JsonSerializer.Serialize(sessionInfo);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"api/SessionInfo/{sessionInfo.SessionId}", content);
            if (response.IsSuccessStatusCode) return RedirectToAction("Index");

            ModelState.AddModelError("", "Error updating session");
            return View(sessionInfo);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteSession(int id)
        {
            if (!IsAdmin()) return RedirectToAction("Index", "Login");

            var response = await _httpClient.GetAsync($"api/SessionInfo/{id}");
            if (!response.IsSuccessStatusCode) return NotFound();

            var json = await response.Content.ReadAsStringAsync();
            var sessionInfo = JsonSerializer.Deserialize<SessionInfo>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return View(sessionInfo);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteSession(SessionInfo sessionInfo)
        {
            if (!IsAdmin()) return RedirectToAction("Index", "Login");

            var response = await _httpClient.DeleteAsync($"api/SessionInfo/{sessionInfo.SessionId}");
            if (response.IsSuccessStatusCode) return RedirectToAction("Index");

            ModelState.AddModelError("", "Error deleting session");
            return View(sessionInfo);
        }
        #endregion

        #region Speakers CRUD

        // List all speakers
        public async Task<IActionResult> Speakers()
        {
            if (!IsAdmin()) return RedirectToAction("Index", "Login");

            var response = await _httpClient.GetAsync("api/SpeakersDetails");
            var speakers = new List<SpeakersDetails>();

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                speakers = JsonSerializer.Deserialize<List<SpeakersDetails>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }

            return View(speakers);
        }

        // GET: Create Speaker
        [HttpGet]
        public IActionResult CreateSpeaker()
        {
            if (!IsAdmin()) return RedirectToAction("Index", "Login");
            return View();
        }

        // POST: Create Speaker
        [HttpPost]
        public async Task<IActionResult> CreateSpeaker(SpeakersDetails speaker)
        {
            if (!IsAdmin()) return RedirectToAction("Index", "Login");

            var json = JsonSerializer.Serialize(speaker);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("api/SpeakersDetails", content);
            if (response.IsSuccessStatusCode) return RedirectToAction("Index", "Login");

            ModelState.AddModelError("", "Error creating speaker");
            return View(speaker);
        }

        // GET: Edit Speaker
        [HttpGet]
        public async Task<IActionResult> EditSpeaker(int id)
        {
            if (!IsAdmin()) return RedirectToAction("Index", "Login");

            var response = await _httpClient.GetAsync($"api/SpeakersDetails/{id}");
            if (!response.IsSuccessStatusCode) return RedirectToAction("Index", "Login");

            var json = await response.Content.ReadAsStringAsync();
            var speaker = JsonSerializer.Deserialize<SpeakersDetails>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return View(speaker);
        }

        // POST: Edit Speaker
        [HttpPost]
        public async Task<IActionResult> EditSpeaker(SpeakersDetails speaker)
        {
            if (!IsAdmin()) return RedirectToAction("Index", "Login");

            var json = JsonSerializer.Serialize(speaker);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"api/SpeakersDetails/{speaker.SpeakerId}", content);
            if (response.IsSuccessStatusCode) return RedirectToAction("Index", "Login");

            ModelState.AddModelError("", "Error updating speaker");
            return RedirectToAction("Index", "Login");
        }

        // GET: Delete Speaker
        [HttpGet]
        public async Task<IActionResult> DeleteSpeaker(int id)
        {
            if (!IsAdmin()) return RedirectToAction("Index", "Login");

            var response = await _httpClient.GetAsync($"api/SpeakersDetails/{id}");
            if (!response.IsSuccessStatusCode) return NotFound();

            var json = await response.Content.ReadAsStringAsync();
            var speaker = JsonSerializer.Deserialize<SpeakersDetails>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return View(speaker);
        }

        // POST: Delete Speaker
        [HttpPost]
        public async Task<IActionResult> DeleteSpeaker(SpeakersDetails speaker)
        {
            if (!IsAdmin()) return RedirectToAction("Index", "Login");

            var response = await _httpClient.DeleteAsync($"api/SpeakersDetails/{speaker.SpeakerId}");
            if (response.IsSuccessStatusCode) return RedirectToAction("Index", "Login");

            ModelState.AddModelError("", "Error deleting speaker");
            return View(speaker);
        }

        #endregion
    }
}
