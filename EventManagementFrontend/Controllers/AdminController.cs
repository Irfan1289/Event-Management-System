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

        // Helper to attach JWT to requests
        private void AttachJwtToken()
        {
            var token = HttpContext.Session.GetString("JWToken");
            _httpClient.DefaultRequestHeaders.Authorization = null;
            if (!string.IsNullOrEmpty(token))
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        }

        // Helper to handle unauthorized responses
        private bool IsUnauthorized(System.Net.Http.HttpResponseMessage response)
        {
            return response.StatusCode == System.Net.HttpStatusCode.Unauthorized ||
                   response.StatusCode == System.Net.HttpStatusCode.Forbidden;
        }

        // Dashboard showing both Events and Sessions
        public async Task<IActionResult> Index()
        {
            AttachJwtToken();

            var eventsResponse = await _httpClient.GetAsync("api/EventDetails");
            if (IsUnauthorized(eventsResponse)) return RedirectToAction("Index", "Login");
            var events = new List<EventDetails>();
            if (eventsResponse.IsSuccessStatusCode)
            {
                var json = await eventsResponse.Content.ReadAsStringAsync();
                events = JsonSerializer.Deserialize<List<EventDetails>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }

            var sessionsResponse = await _httpClient.GetAsync("api/SessionInfo");
            if (IsUnauthorized(sessionsResponse)) return RedirectToAction("Index", "Login");
            var sessions = new List<SessionInfo>();
            if (sessionsResponse.IsSuccessStatusCode)
            {
                var sessionJson = await sessionsResponse.Content.ReadAsStringAsync();
                sessions = JsonSerializer.Deserialize<List<SessionInfo>>(sessionJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }

            var speakersResponse = await _httpClient.GetAsync("api/SpeakersDetails");
            if (IsUnauthorized(speakersResponse)) return RedirectToAction("Index", "Login");
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
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateEvent(EventDetails eventDetails)
        {
            AttachJwtToken();
            var json = JsonSerializer.Serialize(eventDetails);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("api/EventDetails", content);
            if (IsUnauthorized(response)) return RedirectToAction("Index", "Login");
            if (response.IsSuccessStatusCode) return RedirectToAction("Index");

            ModelState.AddModelError("", "Error creating event");
            return View(eventDetails);
        }

        [HttpGet]
        public async Task<IActionResult> EditEvent(int id)
        {
            AttachJwtToken();
            var response = await _httpClient.GetAsync($"api/EventDetails/{id}");
            if (IsUnauthorized(response)) return RedirectToAction("Index", "Login");
            if (!response.IsSuccessStatusCode) return NotFound();

            var json = await response.Content.ReadAsStringAsync();
            var eventDetails = JsonSerializer.Deserialize<EventDetails>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return View(eventDetails);
        }

        [HttpPost]
        public async Task<IActionResult> EditEvent(EventDetails eventDetails)
        {
            AttachJwtToken();
            var json = JsonSerializer.Serialize(eventDetails);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"api/EventDetails/{eventDetails.EventId}", content);
            if (IsUnauthorized(response)) return RedirectToAction("Index", "Login");
            if (response.IsSuccessStatusCode) return RedirectToAction("Index");

            ModelState.AddModelError("", "Error updating event");
            return View(eventDetails);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            AttachJwtToken();
            var response = await _httpClient.GetAsync($"api/EventDetails/{id}");
            if (IsUnauthorized(response)) return RedirectToAction("Index", "Login");
            if (!response.IsSuccessStatusCode) return NotFound();

            var json = await response.Content.ReadAsStringAsync();
            var eventDetails = JsonSerializer.Deserialize<EventDetails>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return View(eventDetails);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteEvent(EventDetails eventDetails)
        {
            AttachJwtToken();
            var response = await _httpClient.DeleteAsync($"api/EventDetails/{eventDetails.EventId}");
            if (IsUnauthorized(response)) return RedirectToAction("Index", "Login");
            if (response.IsSuccessStatusCode) return RedirectToAction("Index");

            ModelState.AddModelError("", "Error deleting event");
            return View(eventDetails);
        }

        #endregion

        #region Session CRUD

        [HttpGet]
        public IActionResult CreateSession()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateSession(SessionInfo sessionInfo)
        {
            AttachJwtToken();
            var json = JsonSerializer.Serialize(sessionInfo);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("api/SessionInfo", content);
            if (IsUnauthorized(response)) return RedirectToAction("Index", "Login");
            if (response.IsSuccessStatusCode) return RedirectToAction("Index");

            ModelState.AddModelError("", "Error creating session");
            return View(sessionInfo);
        }

        [HttpGet]
        public async Task<IActionResult> EditSession(int id)
        {
            AttachJwtToken();
            var response = await _httpClient.GetAsync($"api/SessionInfo/{id}");
            if (IsUnauthorized(response)) return RedirectToAction("Index", "Login");
            if (!response.IsSuccessStatusCode) return NotFound();

            var json = await response.Content.ReadAsStringAsync();
            var sessionInfo = JsonSerializer.Deserialize<SessionInfo>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return View(sessionInfo);
        }

        [HttpPost]
        public async Task<IActionResult> EditSession(SessionInfo sessionInfo)
        {
            AttachJwtToken();
            var json = JsonSerializer.Serialize(sessionInfo);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"api/SessionInfo/{sessionInfo.SessionId}", content);
            if (IsUnauthorized(response)) return RedirectToAction("Index", "Login");
            if (response.IsSuccessStatusCode) return RedirectToAction("Index");

            ModelState.AddModelError("", "Error updating session");
            return View(sessionInfo);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteSession(int id)
        {
            AttachJwtToken();
            var response = await _httpClient.GetAsync($"api/SessionInfo/{id}");
            if (IsUnauthorized(response)) return RedirectToAction("Index", "Login");
            if (!response.IsSuccessStatusCode) return NotFound();

            var json = await response.Content.ReadAsStringAsync();
            var sessionInfo = JsonSerializer.Deserialize<SessionInfo>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return View(sessionInfo);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteSession(SessionInfo sessionInfo)
        {
            AttachJwtToken();
            var response = await _httpClient.DeleteAsync($"api/SessionInfo/{sessionInfo.SessionId}");
            if (IsUnauthorized(response)) return RedirectToAction("Index", "Login");
            if (response.IsSuccessStatusCode) return RedirectToAction("Index");

            ModelState.AddModelError("", "Error deleting session");
            return View(sessionInfo);
        }

        #endregion

        #region Speakers CRUD

        public async Task<IActionResult> Speakers()
        {
            AttachJwtToken();
            var response = await _httpClient.GetAsync("api/SpeakersDetails");
            if (IsUnauthorized(response)) return RedirectToAction("Index", "Login");
            var speakers = new List<SpeakersDetails>();

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
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateSpeaker(SpeakersDetails speaker)
        {
            AttachJwtToken();
            var json = JsonSerializer.Serialize(speaker);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("api/SpeakersDetails", content);
            if (IsUnauthorized(response)) return RedirectToAction("Index", "Login");
            if (response.IsSuccessStatusCode) return RedirectToAction("Index");

            ModelState.AddModelError("", "Error creating speaker");
            return View(speaker);
        }

        [HttpGet]
        public async Task<IActionResult> EditSpeaker(int id)
        {
            AttachJwtToken();
            var response = await _httpClient.GetAsync($"api/SpeakersDetails/{id}");
            if (IsUnauthorized(response)) return RedirectToAction("Index", "Login");
            if (!response.IsSuccessStatusCode) return RedirectToAction("Index", "Login");
            

            var json = await response.Content.ReadAsStringAsync();
            var speaker = JsonSerializer.Deserialize<SpeakersDetails>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return View(speaker);
        }

        [HttpPost]
        public async Task<IActionResult> EditSpeaker(SpeakersDetails speaker)
        {
            AttachJwtToken();
            var json = JsonSerializer.Serialize(speaker);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"api/SpeakersDetails/{speaker.SpeakerId}", content);
            if (IsUnauthorized(response)) return RedirectToAction("Index", "Login");
            if (response.IsSuccessStatusCode) return RedirectToAction("Index");

            ModelState.AddModelError("", "Error updating speaker");
            return View(speaker);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteSpeaker(int id)
        {
            AttachJwtToken();
            var response = await _httpClient.GetAsync($"api/SpeakersDetails/{id}");
            if (IsUnauthorized(response)) return RedirectToAction("Index", "Login");
            if (!response.IsSuccessStatusCode) return NotFound();

            var json = await response.Content.ReadAsStringAsync();
            var speaker = JsonSerializer.Deserialize<SpeakersDetails>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return View(speaker);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteSpeaker(SpeakersDetails speaker)
        {
            AttachJwtToken();
            var response = await _httpClient.DeleteAsync($"api/SpeakersDetails/{speaker.SpeakerId}");
            if (IsUnauthorized(response)) return RedirectToAction("Index", "Login");
            if (response.IsSuccessStatusCode) return RedirectToAction("Index");

            ModelState.AddModelError("", "Error deleting speaker");
            return View(speaker);
        }

        #endregion
    }
}
