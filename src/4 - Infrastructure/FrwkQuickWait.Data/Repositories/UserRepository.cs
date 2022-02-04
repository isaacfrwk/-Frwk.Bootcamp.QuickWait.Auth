using FrwkQuickWait.Data.Context;
using FrwkQuickWait.Domain.Entities;
using FrwkQuickWait.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace FrwkQuickWait.Data.Repositories
{
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        private readonly ApplicationDbContext dbContext;
        public UserRepository(ApplicationDbContext context) : base(context)
        {
            this.dbContext = context;
        }

        public async Task<IEnumerable<User>> Get(string username, string password)
        {
            var query = from user in dbContext.Users
                        where user.Username == username
                           && user.Password == password
                        select user;

            return query.ToList();
        }
    }
}
