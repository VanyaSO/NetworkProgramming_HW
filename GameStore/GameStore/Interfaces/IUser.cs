using GameStore.Models;

namespace GameStore.Interfaces;

public interface IUser
{
    void AddUser(User user);
    User GetUserByEmail(string user);
}