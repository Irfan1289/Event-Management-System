using EventManagement.Context;
using EventManagement.Model;
using System.Collections.Generic;
using System.Linq;

namespace EventManagement.Repository
{
    public class ParticipantEventRepository : IParticipantEventRepository
    {
        private readonly EventDbContext _context;

        public ParticipantEventRepository(EventDbContext context)
        {
            _context = context;
        }

        public IEnumerable<ParticipantEventDetails> GetAll()
        {
            return _context.ParticipantEvents.ToList();
        }

        public ParticipantEventDetails Get(int id)
        {
            return _context.ParticipantEvents.Find(id);
        }

        public void Add(ParticipantEventDetails participantEvent)
        {
            _context.ParticipantEvents.Add(participantEvent);
        }

        public void Update(ParticipantEventDetails participantEvent)
        {
            _context.ParticipantEvents.Update(participantEvent);
        }

        public void Delete(int id)
        {
            var participantEvent = _context.ParticipantEvents.Find(id);
            if (participantEvent != null)
            {
                _context.ParticipantEvents.Remove(participantEvent);
            }
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public EventDetails GetEventById(int eventId)
        {
            return _context.Events.FirstOrDefault(e => e.EventId == eventId);
        }

        public UserInfo GetUserByEmail(string emailId)
        {
            // return _context.UserInfos.FirstOrDefault(u => u.EmailId == emailId);
            return _context.Users.FirstOrDefault(u => u.EmailId == emailId);
        }
    }
}
