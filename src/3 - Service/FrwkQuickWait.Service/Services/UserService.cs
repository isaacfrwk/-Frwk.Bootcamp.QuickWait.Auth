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

        public async Task Save(User user)
        {
            await repository.SaveAsync(user);
        }

        public void Update(User user)
        {
            repository.Update(user);
        }

        public void Delete(User user)
        {
            repository.Delete(user);
        }

        public void DeleteMany(IEnumerable<User> users)
        {
            repository.DeleteMany(users);
        }

        public IEnumerable<User> GetAll()
        {
           return repository.GetAll();
        }
    }
}
