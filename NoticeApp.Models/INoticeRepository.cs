namespace NoticeApp.Models
{
    public interface INoticeRepository : ICrudRepository<Notice>
    {
        Task<List<Notice>> GetPageByParentIdAsync(int pageIndex, int pageSize, int parentId);

        Task<int> GetTotalRecordsCountByParentIdAsync(int parentId);
        Task<int> GetPinnedRecordsByParentIdAsync(int parentId);

        Task<bool> RemoveAllByParentIdAsync(int parentId);
    }
}
