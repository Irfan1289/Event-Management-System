using EventManagement.Model;
using EventManagement.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace ServiceLayer.Controllers
{
    // API route for session info management
    [Route("api/SessionInfo")]
    [ApiController]
    public class SessionInfoController : ControllerBase
    {
        private readonly ISessionInfoRepository _sessionRepo;

        // Constructor with dependency injection for the repository
        public SessionInfoController(ISessionInfoRepository sessionRepo)
        {
            _sessionRepo = sessionRepo;
        }

        // GET: api/SessionInfo
        // Accessible by Admin and Participant roles
        [HttpGet]
        [Authorize(Roles = "Admin, Participant")]
        public IActionResult GetAll()
        {
            return Ok(_sessionRepo.GetAll());
        }

        // GET: api/SessionInfo/{id}
        // Accessible by Admin and Participant roles
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin, Participant")]
        public IActionResult GetById(int id)
        {
            var session = _sessionRepo.Get(id);
            if (session == null) return NotFound();
            return Ok(session);
        }

        // POST: api/SessionInfo
        // Only accessible by Admin role
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Create(SessionInfo session)
        {
            _sessionRepo.Add(session);
            _sessionRepo.Save();
            return CreatedAtAction(nameof(GetById), new { id = session.SessionId }, session);
        }

        // PUT: api/SessionInfo/{id}
        // Only accessible by Admin role
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult Update(int id, SessionInfo session)
        {
            if (id != session.SessionId) return BadRequest();
            _sessionRepo.Update(session);
            _sessionRepo.Save();
            return NoContent();
        }

        // DELETE: api/SessionInfo/{id}
        // Only accessible by Admin role
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            _sessionRepo.Delete(id);
            _sessionRepo.Save();
            return NoContent();
        }
    }
}