using EventManagement.Context;
using EventManagement.Model;

namespace EventManagement.Repository
{
    public class SpeakersRepository : ISpeakersRepository
    {
        private readonly EventDbContext _context;
        public SpeakersRepository(EventDbContext context)
        {
            _context = context;
        }

        public IEnumerable<SpeakersDetails> GetAll()
        {
            return _context.Speakers.ToList();
        }

        public SpeakersDetails Get(int id)
        {
            return _context.Speakers.Find(id);
        }

        public void Add(SpeakersDetails speaker)
        {
            _context.Speakers.Add(speaker);
        }

        public void Update(SpeakersDetails speaker)
        {
            _context.Speakers.Update(speaker);
        }

        public void Delete(int id)
        {
            var speaker = _context.Speakers.Find(id);
            if (speaker != null)
            {
                _context.Speakers.Remove(speaker);
            }
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}