namespace FasterMate.Infrastructure.Common
{
    public interface IRepository<T> : IDisposable
        where T : class
    {
        IQueryable<T> All();

        IQueryable<T> AllAsNoTracking();

        Task AddAsync(T entity);

        void Update(T entity);

        void Delete(T entity);

        Task<int> SaveChangesAsync();
    }
}
