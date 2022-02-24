using FrwkQuickWait.Domain.Entities;

namespace FrwkQuickWait.Domain.Interfaces
{
    public interface ITokenService
    {
        Task<string> GenerateToken(User user);
    }
}
