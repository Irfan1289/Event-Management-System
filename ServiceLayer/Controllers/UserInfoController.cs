using EventManagement.Model;
using EventManagement.Repository;
using Microsoft.AspNetCore.Mvc;

namespace ServiceLayer.Controllers
{
    [Route("api/userinfo")]
    [ApiController]
    public class UserInfoController : ControllerBase
    {
        private readonly IUserRepository _repo;

        public UserInfoController(IUserRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_repo.GetAll());
        }

        [HttpGet("{emailId}")]
        public IActionResult GetById(string emailId)
        {
            var user = _repo.Get(emailId);
            if (user == null) return NotFound();
            return Ok(user);
        }

        [HttpPost]
        public IActionResult Create(UserInfo user)
        {
            _repo.Add(user);
            _repo.Save();
            return CreatedAtAction(nameof(GetById), new { emailId = user.EmailId }, user);
        }

        [HttpPut("{emailId}")]
        public IActionResult Update(string emailId, UserInfo user)
        {
            if (emailId != user.EmailId) return BadRequest();
            _repo.Update(user);
            _repo.Save();
            return NoContent();
        }

        [HttpDelete("{emailId}")]
        public IActionResult Delete(string emailId)
        {
            _repo.Delete(emailId);
            _repo.Save();
            return NoContent();
        }
    }
}
