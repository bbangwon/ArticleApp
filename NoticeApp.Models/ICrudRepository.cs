namespace NoticeApp.Models
{
    public interface ICrudRepository<T>
    {
        Task<T> AddAsync(T model);
        Task<List<T>> GetAllAsync();
        Task<T?> GetByIdAsync(int id);
        Task<bool> UpdateAsync(T model);
        Task<bool> RemoveAsync(int id);

        Task<int> GetTotalRecords();
        Task<List<T>> GetAllOfPageAsync(int pageIndex, int pageSize);       
    }
}
