using EventManagement.Context;
using EventManagement.Model;
using System.Collections.Generic;
using System.Linq;

namespace EventManagement.Repository
{
    public class EventRepository : IEventRepository
    {
        private readonly EventDbContext _context;

        public EventRepository(EventDbContext context)
        {
            _context = context;
        }

        public IEnumerable<EventDetails> GetAll()
        {
            return _context.Events.ToList();
        }

        public EventDetails Get(int id)
        {
            return _context.Events.Find(id);
        }

        public void Add(EventDetails eventDetails)
        {
            _context.Events.Add(eventDetails);
        }
        

        public void Update(EventDetails eventDetails)
        {
            _context.Events.Update(eventDetails);
        }

        public void Delete(int id)
        {
            var eventDetails = _context.Events.Find(id);
            if (eventDetails != null)
            {
                _context.Events.Remove(eventDetails);
            }
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
