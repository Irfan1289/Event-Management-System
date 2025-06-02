using EventManagement.Model;

namespace EventManagement.Repository
{
public interface IUserRepository
{
IEnumerable<UserInfo> GetAll();
UserInfo Get(string emailId);
void Add(UserInfo user);
void Update(UserInfo user);
void Delete(string emailId);
void Save();
}
}