using EventManagement.Model;
using System.Collections.Generic;

namespace EventManagement.Repository
{
    public interface IEventRepository
    {
        IEnumerable<EventDetails> GetAll();
        EventDetails Get(int id);
        void Add(EventDetails eventDetails);
        void Update(EventDetails eventDetails);
        void Delete(int id);
        void Save();
    }
}
