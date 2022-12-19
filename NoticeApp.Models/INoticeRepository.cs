namespace NoticeApp.Models
{
    public interface INoticeRepository : ICrudRepository<Notice>
    {
        Task<List<Notice>> GetAllByParentIdAsync(int pageIndex, int pageSize, int parentId);
    }
}
