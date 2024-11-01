using GameStore.Interfaces;
using GameStore.Models;

namespace GameStore.Repository;

public class UserRepository : IUser
{
    private readonly ApplicationContext _context;

    public UserRepository(ApplicationContext context)
    {
        _context = context;
    }

    public void AddUser(User user)
    {
        _context.Users.Add(user);
        _context.SaveChanges();
    }
    
    public User GetUserByEmail(string email)
    {
        return _context.Users.FirstOrDefault(e => e.Email == email); 
    }
}