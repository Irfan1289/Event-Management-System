using EventManagement.Model;
using EventManagement.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace ServiceLayer.Controllers
{
    // API route for participant event details management
    [Route("api/ParticipantEventDetails")]
    [ApiController]
    public class ParticipantEventDetailsController : ControllerBase
    {
        private readonly IParticipantEventRepository _repo;

        // Constructor with dependency injection for the repository
        public ParticipantEventDetailsController(IParticipantEventRepository repo)
        {
            _repo = repo;
        }

        // GET: api/ParticipantEventDetails
        // Accessible by Admin and Participant roles
        [Authorize(Roles = "Admin, Participant")]
        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_repo.GetAll());
        }

        // GET: api/ParticipantEventDetails/{id}
        // Accessible by any authenticated user (consider restricting if needed)
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var participant = _repo.Get(id);
            if (participant == null) return NotFound();
            return Ok(participant);
        }

        // POST: api/ParticipantEventDetails
        // Allows Admin and Participant to register for an event
        [HttpPost]
        [Authorize(Roles = "Admin, Participant")]
        public IActionResult Create(ParticipantEventDetails participant)
        {
            // Prevent duplicate registration for same user and event
            var existing = _repo.GetAll().FirstOrDefault(p =>
                p.EventId == participant.EventId &&
                p.ParticipantEmailId == participant.ParticipantEmailId);

            if (existing != null)
                return Conflict("Participant already registered for this event.");

            _repo.Add(participant);
            _repo.Save();
            return CreatedAtAction(nameof(GetById), new { id = participant.Id }, participant);
        }

        // PUT: api/ParticipantEventDetails/{id}
        // Only Admin can update participant event details
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult Update(int id, ParticipantEventDetails participant)
        {
            if (id != participant.Id) return BadRequest();
            _repo.Update(participant);
            _repo.Save();
            return NoContent();
        }

        // DELETE: api/ParticipantEventDetails/{id}
        // Only Admin can delete participant event details
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _repo.Delete(id);
            _repo.Save();
            return NoContent();
        }
    }
}