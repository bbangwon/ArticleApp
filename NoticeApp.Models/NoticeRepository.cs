namespace NoticeApp.Models
{
    public class NoticeRepository : INoticeRepository
    {
        public Task<Notice> AddAsync(Notice model)
        {
            throw new NotImplementedException();
        }

        public Task<List<Notice>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<List<Notice>> GetAllByParentIdAsync(int pageIndex, int pageSize, int parentId)
        {
            throw new NotImplementedException();
        }

        public Task<List<Notice>> GetAllOfPageAsync(int pageIndex, int pageSize)
        {
            throw new NotImplementedException();
        }

        public Task<Notice> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetCountAsync()
        {
            throw new NotImplementedException();
        }

        public Task<bool> RemoveAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(Notice model)
        {
            throw new NotImplementedException();
        }
    }
}
