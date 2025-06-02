using EventManagement.Context;
using EventManagement.Model;

namespace EventManagement.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly EventDbContext _context;
        public UserRepository(EventDbContext context)
        {
            _context = context;
        }

        public IEnumerable<UserInfo> GetAll()
        {
            return _context.Users.ToList();
        }

        public UserInfo Get(string emailId)
        {
            return _context.Users.Find(emailId);
        }

        public void Add(UserInfo user)
        {
            _context.Users.Add(user);
        }

        public void Update(UserInfo user)
        {
            _context.Users.Update(user);
        }

        public void Delete(string emailId)
        {
            var user = _context.Users.Find(emailId);
            if (user != null)
            {
                _context.Users.Remove(user);
            }
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}