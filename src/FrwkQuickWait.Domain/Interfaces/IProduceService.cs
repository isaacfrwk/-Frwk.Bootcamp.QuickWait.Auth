using FrwkQuickWait.Domain.Entities;

namespace FrwkQuickWait.Domain.Interfaces
{
    public interface IProduceService
    {
        Task Call(MessageInput message);
    }
}
