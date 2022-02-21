using FrwkQuickWait.Domain.Entities;
using FrwkQuickWait.Domain.Interfaces.Repositories;
using FrwkQuickWait.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace FrwkQuickWait.Infrastructure.Data.Repositories
{
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        private readonly ApplicationDbContext dbContext;
        public UserRepository(ApplicationDbContext context) : base(context)
           =>  this.dbContext = context;

        public async Task<User> Get(string username, string password)
        {
            var query = from user in dbContext.Users
                        where user.Username == username
                           && user.Password == password
                        select user;

            return await query.FirstOrDefaultAsync();
        }

        public async Task<User> GetByCnpj(string password, string cnpj)
        {
            var query = from user in dbContext.Users
                        where user.Password == password
                           && user.CNPJ == cnpj
                        select user;

            return await query.FirstOrDefaultAsync();
        }

        public async Task<User> GetByCpf(string password, string cpf)
        {
            var query = from user in dbContext.Users
                        where user.CPF == cpf
                           && user.Password == password
                        select user;

            return await query.FirstOrDefaultAsync();
        }
    }
}
