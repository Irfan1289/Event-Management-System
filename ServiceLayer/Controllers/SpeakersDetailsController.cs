using EventManagement.Model;
using EventManagement.Repository;
using Microsoft.AspNetCore.Mvc;


namespace ServiceLayer.Controllers
{
    [Route("api/SpeakersDetails")]
    [ApiController]
    public class SpeakersDetailsController : ControllerBase
    {
        private readonly ISpeakersRepository _repo;

        public SpeakersDetailsController(ISpeakersRepository repo)
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
            var speaker = _repo.Get(id);
            if (speaker == null) return NotFound();
            return Ok(speaker);
        }

        [HttpPost]
        public IActionResult Create(SpeakersDetails speaker)
        {
            _repo.Add(speaker);
            _repo.Save();
            return CreatedAtAction(nameof(GetById), new { id = speaker.SpeakerId }, speaker);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, SpeakersDetails speaker)
        {
            if (id != speaker.SpeakerId) return BadRequest();
            _repo.Update(speaker);
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
