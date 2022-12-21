using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace NoticeApp.Models
{
    public class NoticeRepository : INoticeRepository
    {
        private readonly NoticeAppDbContext dbContext;
        private readonly ILogger logger;

        public NoticeRepository(NoticeAppDbContext dbContext, ILoggerFactory loggerFactory)
        {
            this.dbContext = dbContext;
            this.logger = loggerFactory.CreateLogger<NoticeRepository>();
        }

        public async Task<Notice> AddAsync(Notice model)
        {
            this.dbContext.Add(model);
            try
            {
                await this.dbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                logger.LogError($"에러 발생({nameof(AddAsync)}): {e.Message}");
            }

            return model;
        }

        public Task<List<Notice>> GetAllAsync()
        {
            return this.dbContext.Notices
                .OrderByDescending(m => m.Id)
                .ToListAsync();
        }

        public Task<List<Notice>> GetAllByParentIdAsync(int pageIndex, int pageSize, int parentId)
        {
            throw new NotImplementedException();
        }

        public Task<List<Notice>> GetAllOfPageAsync(int pageIndex, int pageSize)
        {
            return this.dbContext.Notices
                .OrderByDescending(m => m.Id)
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public Task<Notice?> GetByIdAsync(int id)
        {
            return this.dbContext.Notices.FindAsync(id).AsTask();
        }

        public Task<int> GetCountAsync()
        {
            return this.dbContext.Notices.CountAsync();
        }

        public async Task<bool> RemoveAsync(int id)
        {
            var notice = GetByIdAsync(id);
            if(notice != null)
            {
                this.dbContext.Remove(notice);
                await this.dbContext.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> UpdateAsync(Notice model)
        {
            var notice = GetByIdAsync((int)model.Id);

            if(notice != null)
            {
                this.dbContext.Update(model);
                await this.dbContext.SaveChangesAsync();
                return true;
            }
            return false;            
        }
    }
}
