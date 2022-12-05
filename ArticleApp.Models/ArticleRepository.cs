namespace ArticleApp.Models
{
    public class ArticleRepository : IArticleRepository
    {
        public Task<Article> AddAsync(Article article)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Article>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Article> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Article> UpdateAsync(Article article)
        {
            throw new NotImplementedException();
        }

        public Task<List<Article>> GetPageAsync(int pageIndex, int pageSize)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetTotalRecordsCountAsync()
        {
            throw new NotImplementedException();
        }
    }
}
