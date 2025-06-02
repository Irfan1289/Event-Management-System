using EventManagement.Model;

namespace EventManagement.Repository
{
public interface ISpeakersRepository
{
IEnumerable<SpeakersDetails> GetAll();
SpeakersDetails Get(int id);
void Add(SpeakersDetails speaker);
void Update(SpeakersDetails speaker);
void Delete(int id);
void Save();
}
}