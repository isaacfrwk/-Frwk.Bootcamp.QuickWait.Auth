using FrwkQuickWait.Domain.Entities;
using FrwkQuickWait.Domain.Interfaces.Repositories;
using FrwkQuickWait.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrwkQuickWait.Service.Services
{
    public class UserService : IUserService
    {
        readonly IUserRepository repository;
        public UserService(IUserRepository repository)
        {
            this.repository = repository;
        }

        public async Task<IEnumerable<User>> GetUser(User user)
        {
           return await repository.Get(user.Username, user.Password);
        }
    }
}
