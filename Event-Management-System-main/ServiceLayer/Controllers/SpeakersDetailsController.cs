using EventManagement.Model;
using EventManagement.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace ServiceLayer.Controllers
{
    // API route for speakers management
    [Route("api/SpeakersDetails")]
    [ApiController]
    public class SpeakersDetailsController : ControllerBase
    {
        private readonly ISpeakersRepository _repo;

        // Constructor with dependency injection for the repository
        public SpeakersDetailsController(ISpeakersRepository repo)
        {
            _repo = repo;
        }

        // GET: api/SpeakersDetails
        // Accessible by Admin and Participant roles
        [Authorize(Roles = "Admin, Participant")]
        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_repo.GetAll());
        }

        // GET: api/SpeakersDetails/{id}
        // Accessible by Admin and Participant roles
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin, Participant")]
        public IActionResult GetById(int id)
        {
            var speaker = _repo.Get(id);
            if (speaker == null) return NotFound();
            return Ok(speaker);
        }

        // POST: api/SpeakersDetails
        // Only accessible by Admin role
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Create(SpeakersDetails speaker)
        {
            _repo.Add(speaker);
            _repo.Save();
            return CreatedAtAction(nameof(GetById), new { id = speaker.SpeakerId }, speaker);
        }

        // PUT: api/SpeakersDetails/{id}
        // Only accessible by Admin role
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult Update(int id, SpeakersDetails speaker)
        {
            if (id != speaker.SpeakerId) return BadRequest();
            _repo.Update(speaker);
            _repo.Save();
            return NoContent();
        }

        // DELETE: api/SpeakersDetails/{id}
        // Only accessible by Admin role
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            _repo.Delete(id);
            _repo.Save();
            return NoContent();
        }
    }
}