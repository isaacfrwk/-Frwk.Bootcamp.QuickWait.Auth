namespace FrwkQuickWait.Domain.Interfaces.Repositories
{
    public interface IRepositoryBase<T> where T : class
    {
        Task SaveAsync(T obj);
        void Update(T obj);
        Task<T> GetById(int id);
        void Delete(T obj);
        IEnumerable<T> GetAll();
        void DeleteMany(IEnumerable<T> obj);
    }
}
