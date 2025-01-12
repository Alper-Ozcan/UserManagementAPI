using UserManagement.Services; 
using UserManagement.Models;

namespace UserManagement.Services
{
    public interface IUserService
    {
        User Authenticate(string email, string password);
        User GetById(int id);
    }
}
