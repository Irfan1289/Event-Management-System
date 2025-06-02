using EventManagement.Model;
using System.Collections.Generic;

namespace EventManagement.Repository
{
    public interface IParticipantEventRepository
    {
        IEnumerable<ParticipantEventDetails> GetAll();
        ParticipantEventDetails Get(int id);
        void Add(ParticipantEventDetails participantEvent);
        void Update(ParticipantEventDetails participantEvent);
        void Delete(int id);
        void Save();
        EventDetails GetEventById(int eventId);
        UserInfo GetUserByEmail(string emailId);
    }
}
