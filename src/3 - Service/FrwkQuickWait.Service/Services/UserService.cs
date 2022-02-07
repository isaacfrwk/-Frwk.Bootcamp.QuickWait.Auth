using FrwkQuickWait.Domain.Entities;
using FrwkQuickWait.Domain.Interfaces.Repositories;
using FrwkQuickWait.Domain.Interfaces.Services;

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
           => await repository.Get(user.Username, user.Password);

        public async Task Save(User user)
        {
            user.Role = "manager";
            await repository.SaveAsync(user);
        }

        public void Update(User user)
        {
            user.Role = "manager";
            repository.Update(user);
        }

        public void Delete(User user)
           => repository.Delete(user);

        public void DeleteMany(IEnumerable<User> users)
            => repository.DeleteMany(users);

        public IEnumerable<User> GetAll()
            => repository.GetAll();
    }
}
