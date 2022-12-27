namespace NoticeApp.Models
{
    public interface INoticeRepository : ICrudRepository<Notice>
    {
        Task<List<Notice>> GetAllByParentIdAsync(int pageIndex, int pageSize, int parentId);

        Task<int> GetTotalRecords(int parentId);
        Task<int> GetPinnedRecords(int parentId);

        Task<bool> RemoveAllByParentIdAsync(int parentId);
    }
}
