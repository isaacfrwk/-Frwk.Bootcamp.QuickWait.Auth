using FrwkQuickWait.Domain.Entities;

namespace FrwkQuickWait.Domain.Interfaces.Services
{
    public interface ITokenService
    {
        string GenerateToken(User user);
    }
}
