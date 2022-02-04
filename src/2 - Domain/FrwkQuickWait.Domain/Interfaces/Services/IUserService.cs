using FrwkQuickWait.Domain.Entities;

namespace FrwkQuickWait.Domain.Interfaces.Services
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetUser(User user);
        Task Save(User user);
        void Update(User user);
        void Delete(User user);
        IEnumerable<User> GetAll();
        void DeleteMany(IEnumerable<User> users);
    }
}
