namespace BWPager
{
    public interface IBWPagerRepository<T>
    {
        Task<List<T>> GetPageAsync(int pageIndex, int pageSize);
        Task<int> GetTotalRecordsCountAsync();
    }
}
