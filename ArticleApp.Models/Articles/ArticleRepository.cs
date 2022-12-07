using Microsoft.EntityFrameworkCore;

namespace ArticleApp.Models
{
    public class ArticleRepository : IArticleRepository
    {
        private readonly ArticleAppDbContext context;

        public ArticleRepository(ArticleAppDbContext context)
        {
            this.context = context;
        }

        public async Task<Article> AddAsync(Article model)
        {
            context.Articles.Add(model);
            await context.SaveChangesAsync();
            return model;
        }
        public async Task<List<Article>> GetAllAsync()
        {
            return await context.Articles.ToListAsync();
        }
        public async Task<Article?> GetByIdAsync(int id)
        {
            return await context.Articles.FindAsync(id);
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);

            if(entity != null)
            {
                context.Articles.Remove(entity);
                await context.SaveChangesAsync();
            }           
        }

        public async Task<Article> UpdateAsync(Article model)
        {
            context.Articles.Update(model);
            await context.SaveChangesAsync();
            return model;
        }

        public async Task<List<Article>> GetPageAsync(int pageIndex, int pageSize)
        {
            return await context.Articles
                .OrderByDescending(m => m.Id)
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<int> GetTotalRecordsCountAsync()
        {
            return await context.Articles.CountAsync();
        }
    }
}
