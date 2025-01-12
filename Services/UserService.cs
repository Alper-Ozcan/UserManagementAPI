using System.Linq;
using UserManagement.Data;
using UserManagement.Models;

namespace UserManagement.Services
{
    public class UserService : IUserService
    {
        // private readonly List<User> _users = new List<User>
        // {
        //     new User { Id = 1, Email = "test1@example.com", Password = "1" } // Dummy user for example
        // };
        private readonly List<User> _users = UserData.Users;

        public User Authenticate(string email, string password)
        {
            var user = _users.SingleOrDefault(u => u.Email == email && u.Password == password);
            if (user == null)
            {
                throw new ArgumentException("Invalid email or password");
            }

            // Kullanıcının rol bilgilerini kontrol edin
            if (string.IsNullOrEmpty(user.Role))
            {
                throw new ArgumentException("User role is not assigned");
            }

            return user;
        }


        public User GetById(int id)
        {
            var user = _users.SingleOrDefault(u => u.Id == id);
            if (user == null)
            {
                throw new ArgumentException("User not found");
            }
            return user;
        }

    }
}
