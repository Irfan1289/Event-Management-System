using EventManagement.Model;

namespace EventManagement.Repository
{
    public interface ISessionInfoRepository
{
    IEnumerable<SessionInfo> GetAll();
    SessionInfo Get(int id);
    void Add(SessionInfo session);
    void Update(SessionInfo session);
    void Delete(int id);
    void Save();
}

}