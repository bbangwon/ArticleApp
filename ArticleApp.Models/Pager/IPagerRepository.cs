namespace BWBlazor
{
    public interface IPagerRepository<T>
    {
        Task<List<T>> GetPageAsync(int pageIndex, int pageSize);
        Task<int> GetTotalRecordsCountAsync();
    }
}
