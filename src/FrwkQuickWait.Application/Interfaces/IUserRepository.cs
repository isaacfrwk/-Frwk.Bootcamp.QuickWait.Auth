using FrwkQuickWait.Domain.Entities;

namespace FrwkQuickWait.Application.Interfaces
{
    public interface IUserRepository : IRepositoryBase<User>
    {
        Task<User> Get(string username, string password);
        Task<User> GetByCnpj(string password, string cnpj);
        Task<User> GetByCpf(string password, string cpf);
    }
}
