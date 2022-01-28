using FrwkQuickWait.Domain.Entities;
using FrwkQuickWait.Domain.Interfaces.Repositories;
using System.Collections.Generic;

namespace FrwkQuickWait.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        public async Task<IEnumerable<User>> Get(string username, string password)
        {
            var users = new List<User>
            {
                new User { Id = 1, Username = "Unimed", Password = "uni1234", Role = "manager" },
                new User { Id = 2, Username = "Thiago", Password = "thi1234", Role = "employee" }
            };
            return users.Where(x => x.Username.ToLower() == username.ToLower() && x.Password.ToLower() == password.ToLower());
        }
    }
}
