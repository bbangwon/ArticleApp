namespace NoticeApp.Models
{
    public interface INoticeRepository : ICrudRepository<Notice>
    {
        Task<List<Notice>> GetPageByParentIdAsync(int pageIndex, int pageSize, int parentId);

        Task<int> GetTotalRecordsCountByParentIdAsync(int parentId);
        Task<int> GetPinnedRecordsByParentIdAsync(int parentId);

        Task<bool> RemoveAllByParentIdAsync(int parentId);

        Task<int> GetTotalRecordsCountWithSearchQueryAsync(string searchQuery);
        Task<List<Notice>> GetPageWithSearchQueryAsync(int pageIndex, int pageSize, string searchQuery);

        Task<int> GetTotalRecordsCountByParentIdWithSearchQueryAsync(int parentId, string searchQuery);
        Task<List<Notice>> GetPageByParentIdWithSearchQueryAsync(int pageIndex, int pageSize, int parentId, string searchQuery);

    }
}
