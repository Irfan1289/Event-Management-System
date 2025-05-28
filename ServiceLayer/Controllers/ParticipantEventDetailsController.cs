using EventManagement.Model;
using EventManagement.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace ServiceLayer.Controllers
{
    [Route("api/ParticipantEventDetails")]
    [ApiController]
    public class ParticipantEventDetailsController : ControllerBase
    {
        private readonly IParticipantEventRepository _repo;

        public ParticipantEventDetailsController(IParticipantEventRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_repo.GetAll());
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var participant = _repo.Get(id);
            if (participant == null) return NotFound();
            return Ok(participant);
        }

        [HttpPost]
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

        [HttpPut("{id}")]
        public IActionResult Update(int id, ParticipantEventDetails participant)
        {
            if (id != participant.Id) return BadRequest();
            _repo.Update(participant);
            _repo.Save();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _repo.Delete(id);
            _repo.Save();
            return NoContent();
        }
    }
}
