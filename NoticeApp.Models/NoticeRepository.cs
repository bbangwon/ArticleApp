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
            //현재테이블의 Ref의 Max값 가져오기
            #region Reply 추가 기능
            int maxRef = 1;

            if(this.dbContext.Notices.Count() > 0)
            {
                int? max = this.dbContext.Notices.Max(x => x.Ref);
                if (max.HasValue)
                    maxRef = max.Value + 1;
            }

            model.Ref = maxRef;
            model.Step = 0;
            model.RefOrder = 0; 
            #endregion

            try
            {
                this.dbContext.Add(model);
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

        public Task<List<Notice>> GetPageByParentKeyAsync(int pageIndex, int pageSize, string parentKey)
        {
            return this.dbContext.Notices
                .Where(m => m.ParentKey == parentKey)
                .OrderByDescending(m => m.Id)
                //.Include(m => m.NoticesComments)
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public Task<int> GetTotalRecordsCountByParentKeyAsync(string parentKey)
        {
            return this.dbContext.Notices.Where(m => m.ParentKey == parentKey).CountAsync();
        }

        public Task<int> GetTotalRecordsCountByParentKeyWithSearchQueryAsync(string parentKey, string searchQuery)
        {
            return this.dbContext.Notices
                .Where(m => m.ParentKey == parentKey)
                .Where(m =>
                    (m.Name != null && EF.Functions.Like(m.Name, $"%{searchQuery}%")) ||
                    (m.Title != null && m.Title.Contains(searchQuery)) ||
                    (m.Content != null && m.Content.Contains(searchQuery)))
            .CountAsync();
        }

        public Task<List<Notice>> GetPageByParentKeyWithSearchQueryAsync(int pageIndex, int pageSize, string parentKey, string searchQuery)
        {
            return this.dbContext.Notices
                .Where(m => m.ParentKey == parentKey)
                .Where(m =>
                    (m.Name != null && EF.Functions.Like(m.Name, $"%{searchQuery}%")) ||
                    (m.Title != null && m.Title.Contains(searchQuery)) ||
                    (m.Content != null && m.Content.Contains(searchQuery)))
                .OrderByDescending(m => m.Id)
                //.Include(m => m.NoticesComments)
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<(List<Notice> notices, int totalCount)> GetNotices<TParentIdentifier>(int pageIndex, int pageSize, string searchField, string searchQuery, string sortOrder, TParentIdentifier parentIdentifier)
        {
            var items = this.dbContext.Notices.Select(m => m);

            if(parentIdentifier is int parentId && parentId != 0)
            {
                items = items.Where(m => m.ParentId == parentId);
            }
            else if(parentIdentifier is string parentKey && !string.IsNullOrEmpty(parentKey))
            {
                items = items.Where(m => m.ParentKey == parentKey);
            }

            if(!string.IsNullOrEmpty(searchQuery))
            {
                if(searchField == "Name")
                {
                    
                    items = items.Where(m => !string.IsNullOrEmpty(m.Name) && m.Name.Contains(searchQuery));
                }
                else if(searchField == "Title") 
                {
                    items = items.Where(m => !string.IsNullOrEmpty(m.Title) && m.Title.Contains(searchQuery));
                }
                else
                {
                    items = items.Where(m =>(!string.IsNullOrEmpty(m.Name) && m.Name.Contains(searchQuery)) || (!string.IsNullOrEmpty(m.Title) && m.Title.Contains(searchQuery)));
                }
            }

            var totalCount = await items.CountAsync();

            switch(sortOrder)
            {
                case "Name":
                    items = items.OrderBy(m => m.Name);
                    break;
                case "NameDesc":
                    items = items.OrderByDescending(m => m.Name);
                    break;
                case "Title":
                    items = items.OrderBy(m => m.Title);
                    break;
                case "TitleDesc":
                    items = items.OrderByDescending(m => m.Title);
                    break;
                default:
                    items = items.OrderByDescending(m => m.Ref).ThenBy(m => m.RefOrder);
                    break;
            }

            items = items.Skip(pageIndex * pageSize).Take(pageSize);

            return (await items.ToListAsync(), totalCount);
        }

        public async Task<Notice> AddAsync(Notice model, int parentRef, int parentStep, int parentOrder)
        {
            //비집고 들어갈 자리
            var replys = await this.dbContext.Notices.Where(m => m.Ref == parentRef && m.RefOrder > parentOrder).ToListAsync();
            foreach (var item in replys)
            {
                item.RefOrder++;
                try
                {
                    this.dbContext.Update(item);
                    await this.dbContext.SaveChangesAsync();
                }
                catch (Exception e)
                {
                    logger.LogError($"ERROR ({nameof(AddAsync)}): {e.Message}");
                }
            }

            model.Ref = parentRef;
            model.Step = parentStep + 1;
            model.RefOrder = parentOrder + 1;

            try
            {
                this.dbContext.Add(model);
                await this.dbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                logger.LogError($"ERROR ({nameof(AddAsync)}): {e.Message}");
            }

            return model;
        }
    }
}
