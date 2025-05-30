using EventManagement.Model;
using EventManagement.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace ServiceLayer.Controllers
{
    // API route for event details management
    [Route("api/EventDetails")]
    [ApiController]
    public class EventDetailsController : ControllerBase
    {
        private readonly IEventRepository _eventRepository;

        // Constructor with dependency injection for the repository
        public EventDetailsController(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }

        // GET: api/EventDetails
        // Accessible by Admin and Participant roles
        [Authorize(Roles = "Admin, Participant")]
        [HttpGet]
        public IActionResult GetAll()
        {
            var events = _eventRepository.GetAll();
            return Ok(events);
        }

        // GET: api/EventDetails/{id}
        // Accessible by Admin and Participant roles
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin, Participant")]
        public IActionResult GetById(int id)
        {
            var evt = _eventRepository.Get(id);
            if (evt == null) return NotFound();
            return Ok(evt);
        }

        // POST: api/EventDetails
        // Only accessible by Admin role
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Create(EventDetails eventDetails)
        {
            _eventRepository.Add(eventDetails);
            _eventRepository.Save();
            return CreatedAtAction(nameof(GetById), new { id = eventDetails.EventId }, eventDetails);
        }

        // PUT: api/EventDetails/{id}
        // Only accessible by Admin role
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult Update(int id, EventDetails eventDetails)
        {
            if (id != eventDetails.EventId) return BadRequest();
            _eventRepository.Update(eventDetails);
            _eventRepository.Save();
            return NoContent();
        }

        // DELETE: api/EventDetails/{id}
        // Only accessible by Admin role
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            var evt = _eventRepository.Get(id);
            if (evt == null)
                return NotFound();

            _eventRepository.Delete(id);
            _eventRepository.Save();

            return NoContent();
        }
    }
}