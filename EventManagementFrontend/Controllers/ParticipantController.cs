using EventManagement.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Net.Http;
using System.Text.Json;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace EventManagementFrontend.Controllers
{
    public class ParticipantController : BaseController
    {
        private readonly HttpClient _httpClient;

        public ParticipantController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new System.Uri("http://localhost:5199/");
        }

        private bool IsParticipant()
        {
            return HttpContext.Session.GetString("UserRole") == "Participant";
        }

        // Main participant page showing events and registration status
        public async Task<IActionResult> Index()
        {
            if (!IsParticipant()) return RedirectToAction("Index", "Login");

            var eventsResponse = await _httpClient.GetAsync("api/EventDetails");
            var events = new List<EventDetails>();
            if (eventsResponse.IsSuccessStatusCode)
            {
                var eventsJson = await eventsResponse.Content.ReadAsStringAsync();
                events = JsonSerializer.Deserialize<List<EventDetails>>(eventsJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }

            var sessionsResponse = await _httpClient.GetAsync("api/SessionInfo");
            var sessions = new List<SessionInfo>();
            if (sessionsResponse.IsSuccessStatusCode)
            {
                var sessionsJson = await sessionsResponse.Content.ReadAsStringAsync();
                sessions = JsonSerializer.Deserialize<List<SessionInfo>>(sessionsJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }

            var participantRegsResponse = await _httpClient.GetAsync("api/ParticipantEventDetails");
            var registrations = new List<ParticipantEventDetails>();
            if (participantRegsResponse.IsSuccessStatusCode)
            {
                var regsJson = await participantRegsResponse.Content.ReadAsStringAsync();
                registrations = JsonSerializer.Deserialize<List<ParticipantEventDetails>>(regsJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }

            ViewBag.Sessions = sessions;
            ViewBag.Registrations = registrations;
            ViewBag.UserEmail = HttpContext.Session.GetString("EmailId");

            // Pass message from TempData if any
            ViewBag.Message = TempData["Message"];
            ViewBag.Error = TempData["Error"];

            return View(events);
        }

        // GET Register page to confirm registration for an event
        [HttpGet]
        public async Task<IActionResult> Register(int eventId)
        {
            if (!IsParticipant()) return RedirectToAction("Index", "Login");

            var eventResponse = await _httpClient.GetAsync($"api/EventDetails/{eventId}");
            if (!eventResponse.IsSuccessStatusCode)
            {
                return NotFound();
            }

            var eventJson = await eventResponse.Content.ReadAsStringAsync();
            var evt = JsonSerializer.Deserialize<EventDetails>(eventJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return View(evt);
        }

        // POST Register action to submit registration
        [HttpPost]
        public async Task<IActionResult> RegisterConfirmed(int eventId)
        {
            if (!IsParticipant()) return RedirectToAction("Index", "Login");

            string userEmail = HttpContext.Session.GetString("EmailId");
            if (string.IsNullOrEmpty(userEmail)) return RedirectToAction("Index", "Login");

            var registration = new ParticipantEventDetails
            {
                ParticipantEmailId = userEmail,
                EventId = eventId,
                IsAttended = "No"
            };

            var json = JsonSerializer.Serialize(registration);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("api/ParticipantEventDetails", content);
            if (response.IsSuccessStatusCode)
            {
                TempData["Message"] = "Successfully registered for the event.";
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.Conflict)
            {
                TempData["Error"] = "You are already registered for this event.";
            }
            else
            {
                TempData["Error"] = "Failed to register for the event.";
            }

            return RedirectToAction("Index");
        }
    }
}
