using FrwkQuickWait.Domain.Entities;

namespace FrwkQuickWait.Domain.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> Get(string username, string password);
    }
}
