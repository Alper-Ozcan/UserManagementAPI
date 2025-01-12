using UserManagement.Models; 
using UserManagement.Data; 

namespace UserManagement.Data
{
    public static class UserData
    {
        public static List<User> Users = new List<User>
        {
            new User { Id = 1, Email = "test1@example.com", Password = "1", Role = "Admin" },
            new User { Id = 2, Email = "test2@example.com", Password = "2", Role = "User" }
        };
    }
}
