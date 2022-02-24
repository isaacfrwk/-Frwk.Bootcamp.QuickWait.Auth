using FrwkQuickWait.Application.Interfaces;
using FrwkQuickWait.Domain.Entities;
using FrwkQuickWait.Domain.Interfaces;

namespace FrwkQuickWait.Service.Services
{
    public class UserService : IUserService
    {
        readonly IUserRepository repository;
        public UserService(IUserRepository repository) =>
            this.repository = repository;
        

        public async Task<User> GetUser(User user) =>
            await repository.Get(user.Username, user.Password);

        public async Task Save(User user) =>
            await repository.SaveAsync(user);

        public void Update(User user) =>
            repository.Update(user);

        public void Delete(User user) =>
            repository.Delete(user);

        public void DeleteMany(IEnumerable<User> users) =>
            repository.DeleteMany(users);

        public IEnumerable<User> GetAll() =>
            repository.GetAll();
    }
}
