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
            if (notice != null)
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

        public Task<int> GetTotalRecordsCountWithSearchQueryAsync(string searchQuery)
        {
            return this.dbContext.Notices
                .Where(m =>
                    (m.Name != null && m.Name.Contains(searchQuery)) ||
                    (m.Title != null && m.Title.Contains(searchQuery)) ||
                    (m.Content != null && m.Content.Contains(searchQuery)))
                .CountAsync();
        }

        public Task<List<Notice>> GetPageWithSearchQueryAsync(int pageIndex, int pageSize, string searchQuery)
        {
            return this.dbContext.Notices
                .Where(m => 
                    (m.Name != null && m.Name.Contains(searchQuery)) || 
                    (m.Title != null && m.Title.Contains(searchQuery)) ||
                    (m.Content != null && m.Content.Contains(searchQuery)))
                .OrderByDescending(m => m.Id)
                //.Include(m => m.NoticesComments)
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public Task<int> GetTotalRecordsCountByParentIdWithSearchQueryAsync(int parentId, string searchQuery)
        {
            return this.dbContext.Notices
                .Where(m => m.ParentId == parentId)
                .Where(m =>
                    (m.Name != null && m.Name.Contains(searchQuery)) ||
                    (m.Title != null && m.Title.Contains(searchQuery)) ||
                    (m.Content != null && m.Content.Contains(searchQuery)))
            .CountAsync();
        }

        public Task<List<Notice>> GetPageByParentIdWithSearchQueryAsync(int pageIndex, int pageSize, int parentId, string searchQuery)
        {
            return this.dbContext.Notices
                .Where(m => m.ParentId == parentId)
                .Where(m =>
                    (m.Name != null && m.Name.Contains(searchQuery)) ||
                    (m.Title != null && m.Title.Contains(searchQuery)) ||
                    (m.Content != null && m.Content.Contains(searchQuery)))
                .OrderByDescending(m => m.Id)
                //.Include(m => m.NoticesComments)
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public Task<SortedList<int, int>> GetMonthlyCreateCountAsync()
        {
            SortedList<int, int> createCounts = new SortedList<int, int>();

            //초기화
            for (int i = 1; i <= 12; i++)
            {
                createCounts.Add(i, 0);
            }

            //현재 달부터 12개월 전까지 반복
            for (int i = 0; i < 12; i++)
            {
                var current = DateTime.Now.AddMonths(-i);
                var count = this.dbContext.Notices
                    .AsEnumerable()     // 메모리를 많이 사용할수 있으므로 AsEnumerable
                    .Where(m => 
                Convert.ToDateTime(m.Created).Month == current.Month && 
                Convert.ToDateTime(m.Created).Year == current.Year)
                    .Count();

                createCounts[current.Month] = count;
            }
            return Task.FromResult(createCounts);
        }
    }
}
