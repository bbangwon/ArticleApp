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
                logger.LogError($"ERROR ({nameof(AddAsync)}): {e.Message}");
            }

            return model;
        }

        public Task<List<Notice>> GetAllAsync()
        {
            return this.dbContext.Notices
                .OrderByDescending(m => m.Id)
                //.Include(m => m.NoticeComments)
                .ToListAsync();
        }



        public Task<List<Notice>> GetPageByParentIdAsync(int pageIndex, int pageSize, int parentId)
        {
            return this.dbContext.Notices
                .Where(m => m.ParentId == parentId)
                .OrderByDescending(m => m.Id)
                //.Include(m => m.NoticesComments)
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public Task<Notice?> GetByIdAsync(int id)
        {
            return this.dbContext.Notices
                //.Include(m => m.NoticesComments)
                .FindAsync(id)
                .AsTask();
        }

        public Task<int> GetTotalRecordsCountByParentIdAsync(int parentId)
        {
            return this.dbContext.Notices.Where(m => m.ParentId == parentId).CountAsync();
        }

        public Task<int> GetPinnedRecordsByParentIdAsync(int parentId)
        {
            return this.dbContext.Notices.Where(m => m.ParentId == parentId && m.IsPinned == true).CountAsync();
        }

        public async Task<bool> RemoveAsync(int id)
        {
            var notice = await GetByIdAsync(id);
            if(notice != null)
            {
                this.dbContext.Remove(notice);
                try
                {
                    return await this.dbContext.SaveChangesAsync() > 0;
                }
                catch (Exception e)
                {
                    logger.LogError($"ERROR ({nameof(RemoveAsync)}): {e.Message}");
                }
            }
            return false;
        }

        public async Task<bool> UpdateAsync(Notice model)
        {
            try
            {
                this.dbContext.Update(model);
                return await this.dbContext.SaveChangesAsync() > 0;
            }
            catch (Exception e)
            {
                logger.LogError($"ERROR ({nameof(UpdateAsync)}): {e.Message}");
            }
            return false;            
        }

        public async Task<bool> RemoveAllByParentIdAsync(int parentId)
        {
            try
            {
                var models = await dbContext.Notices.Where(m => m.ParentId == parentId).ToListAsync();

                foreach (var model in models)
                {
                    dbContext.Notices.Remove(model);
                }

                return await dbContext.SaveChangesAsync() > 0;
            }
            catch (Exception e)
            {
                logger.LogError($"ERROR ({nameof(RemoveAllByParentIdAsync)}): {e.Message}");
            }

            return false;
        }

        public Task<List<Notice>> GetPageAsync(int pageIndex, int pageSize)
        {
            return this.dbContext.Notices
                .OrderByDescending(m => m.Id)
                //.Include(m => m.NoticesComments)
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public Task<int> GetTotalRecordsCountAsync()
        {
            return this.dbContext.Notices.CountAsync();
        }
    }
}
