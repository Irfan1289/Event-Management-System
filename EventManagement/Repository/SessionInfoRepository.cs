using EventManagement.Context;
using EventManagement.Model;
using Microsoft.EntityFrameworkCore;

namespace EventManagement.Repository
{
    public class SessionInfoRepository : ISessionInfoRepository
    {
        private readonly EventDbContext _context;

        public SessionInfoRepository(EventDbContext context)
        {
            _context = context;
        }

        public IEnumerable<SessionInfo> GetAll()
        {
            return _context.Sessions
                .Include(s => s.Event)
                .Include(s => s.Speaker)
                .ToList();
        }

        public SessionInfo Get(int id)
        {
#pragma warning disable CS8603 // Possible null reference return.
            return _context.Sessions
                .Include(s => s.Event)
                .Include(s => s.Speaker)
                .FirstOrDefault(s => s.SessionId == id);
#pragma warning restore CS8603 // Possible null reference return.
        }

        public void Add(SessionInfo session)
        {
            _context.Sessions.Add(session);
        }

        public void Update(SessionInfo session)
        {
            _context.Sessions.Update(session);
        }

        public void Delete(int id)
        {
            var session = _context.Sessions.Find(id);
            if (session != null)
            {
                _context.Sessions.Remove(session);
            }
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}