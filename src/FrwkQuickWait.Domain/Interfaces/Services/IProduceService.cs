using FrwkQuickWait.Domain.Entities;

namespace FrwkQuickWait.Domain.Interfaces.Services
{
    public interface IProduceService
    {
        Task Call(MessageInput message);
    }
}
