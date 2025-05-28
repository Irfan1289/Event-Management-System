using EventManagement.Model;
using EventManagement.Repository;
using Microsoft.AspNetCore.Mvc;

namespace ServiceLayer.Controllers
{
    [Route("api/EventDetails")]
    [ApiController]
    public class EventDetailsController : ControllerBase
    {
        private readonly IEventRepository _eventRepository;

        public EventDetailsController(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var events = _eventRepository.GetAll();
            return Ok(events);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var evt = _eventRepository.Get(id);
            if (evt == null) return NotFound();
            return Ok(evt);
        }

        [HttpPost]
        public IActionResult Create(EventDetails eventDetails)
        {
            _eventRepository.Add(eventDetails);
            _eventRepository.Save();
            return CreatedAtAction(nameof(GetById), new { id = eventDetails.EventId }, eventDetails);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, EventDetails eventDetails)
        {
            if (id != eventDetails.EventId) return BadRequest();
            _eventRepository.Update(eventDetails);
            _eventRepository.Save();
            return NoContent();
        }

       [HttpDelete("{id}")]
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
