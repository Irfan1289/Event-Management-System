using EventManagement.Model;
using EventManagement.Repository;
using Microsoft.AspNetCore.Mvc;

namespace ServiceLayer.Controllers
{
    [Route("api/SessionInfo")]
    [ApiController]
    public class SessionInfoController : ControllerBase
    {
        private readonly ISessionInfoRepository _sessionRepo;

        public SessionInfoController(ISessionInfoRepository sessionRepo)
        {
            _sessionRepo = sessionRepo;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_sessionRepo.GetAll());
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var session = _sessionRepo.Get(id);
            if (session == null) return NotFound();
            return Ok(session);
        }

        [HttpPost]
        public IActionResult Create(SessionInfo session)
        {
            _sessionRepo.Add(session);
            _sessionRepo.Save();
            return CreatedAtAction(nameof(GetById), new { id = session.SessionId }, session);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, SessionInfo session)
        {
            if (id != session.SessionId) return BadRequest();
            _sessionRepo.Update(session);
            _sessionRepo.Save();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _sessionRepo.Delete(id);
            _sessionRepo.Save();
            return NoContent();
        }
    }
}
