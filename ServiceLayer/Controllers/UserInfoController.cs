using EventManagement.Model;
using EventManagement.Repository;
using Microsoft.AspNetCore.Mvc;

namespace ServiceLayer.Controllers
{
    // API route for user information management
    [Route("api/userinfo")]
    [ApiController]
    public class UserInfoController : ControllerBase
    {
        private readonly IUserRepository _repo;

        // Constructor with dependency injection for the repository
        public UserInfoController(IUserRepository repo)
        {
            _repo = repo;
        }

        // GET: api/userinfo
        // Returns all users (public endpoint)
        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_repo.GetAll());
        }

        // GET: api/userinfo/{emailId}
        // Returns a user by emailId (public endpoint)
        [HttpGet("{emailId}")]
        public IActionResult GetById(string emailId)
        {
            var user = _repo.Get(emailId);
            if (user == null) return NotFound();
            return Ok(user);
        }

        // POST: api/userinfo
        // Creates a new user (public endpoint, used for registration)
        [HttpPost]
        public IActionResult Create(UserInfo user)
        {
            _repo.Add(user);
            _repo.Save();
            return CreatedAtAction(nameof(GetById), new { emailId = user.EmailId }, user);
        }

        // PUT: api/userinfo/{emailId}
        // Updates an existing user (public endpoint, but you may want to restrict this)
        [HttpPut("{emailId}")]
        public IActionResult Update(string emailId, UserInfo user)
        {
            if (emailId != user.EmailId) return BadRequest();
            _repo.Update(user);
            _repo.Save();
            return NoContent();
        }

        // DELETE: api/userinfo/{emailId}
        // Deletes a user (public endpoint, but you may want to restrict this)
        [HttpDelete("{emailId}")]
        public IActionResult Delete(string emailId)
        {
            _repo.Delete(emailId);
            _repo.Save();
            return NoContent();
        }
    }
}