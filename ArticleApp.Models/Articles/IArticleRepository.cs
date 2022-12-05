using BWBlazor;

namespace ArticleApp.Models.Articles
{
    public interface IArticleRepository
        : IPagerRepository<Article>
    {
        Task<Article> AddAsync(Article article);
        Task<List<Article>> GetAllAsync();
        Task<Article> GetByIdAsync(int id);
        Task<Article> UpdateAsync(Article article);
        Task DeleteAsync(int id);
    }
}
