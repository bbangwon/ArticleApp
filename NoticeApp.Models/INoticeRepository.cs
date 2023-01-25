namespace NoticeApp.Models
{
    public interface INoticeRepository : ICrudRepository<Notice>
    {
        Task<List<Notice>> GetPageByParentIdAsync(int pageIndex, int pageSize, int parentId);
        Task<int> GetTotalRecordsCountByParentIdAsync(int parentId);

        Task<List<Notice>> GetPageByParentKeyAsync(int pageIndex, int pageSize, string parentKey);
        Task<int> GetTotalRecordsCountByParentKeyAsync(string parentKey);


        Task<int> GetTotalRecordsCountWithSearchQueryAsync(string searchQuery);
        Task<List<Notice>> GetPageWithSearchQueryAsync(int pageIndex, int pageSize, string searchQuery);

        
        Task<int> GetTotalRecordsCountByParentIdWithSearchQueryAsync(int parentId, string searchQuery);
        Task<List<Notice>> GetPageByParentIdWithSearchQueryAsync(int pageIndex, int pageSize, int parentId, string searchQuery);

        Task<int> GetTotalRecordsCountByParentKeyWithSearchQueryAsync(string parentKey, string searchQuery);
        Task<List<Notice>> GetPageByParentKeyWithSearchQueryAsync(int pageIndex, int pageSize, string parentKey, string searchQuery);


        Task<SortedList<int, int>> GetMonthlyCreateCountAsync();

        Task<int> GetPinnedRecordsByParentIdAsync(int parentId);
        Task<bool> RemoveAllByParentIdAsync(int parentId);

        Task<(List<Notice> notices, int totalCount)> GetNotices<TParentIdentifier>(int pageIndex, int pageSize, string searchField, string searchQuery, string sortOrder, TParentIdentifier parentIdentifier);
    }
}
