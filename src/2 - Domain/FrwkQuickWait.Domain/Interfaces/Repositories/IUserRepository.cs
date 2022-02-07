using FrwkQuickWait.Domain.Entities;

namespace FrwkQuickWait.Domain.Interfaces.Repositories
{
    public interface IUserRepository : IRepositoryBase<User>
    {
        Task<IEnumerable<User>> Get(string username, string password);
        Task<User> GetByCnpj(string password, string cnpj);
        Task<User> GetByCpf(string password, string cpf);
    }
}
