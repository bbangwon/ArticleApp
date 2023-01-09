using BWShared;

namespace ArticleApp.Models
{
    public interface IArticleRepository
        : IBWPagerRepository<Article>
    {
        Task<Article> AddAsync(Article model);
        Task<List<Article>> GetAllAsync();
        Task<Article?> GetByIdAsync(int id);
        Task<Article> UpdateAsync(Article model);
        Task DeleteAsync(int id);
    }
}
