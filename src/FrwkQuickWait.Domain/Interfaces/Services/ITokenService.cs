using FrwkQuickWait.Domain.Entities;

namespace FrwkQuickWait.Domain.Interfaces.Services
{
    public interface ITokenService
    {
        Task<string> GenerateToken(User user);
    }
}
