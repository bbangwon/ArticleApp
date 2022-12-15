using ArticleApp.Models;
using Microsoft.AspNetCore.Components;

namespace ArticleApp.Pages.Articles
{
    public partial class Create
    {
        [Inject]
        NavigationManager? NavigationManager { get; set; }

        [Inject]
        IArticleRepository? ArticleRepository { get; set; }

        public Article Model { get; set; } = new Article();

        protected async Task btnSubmit_Click()
        {
            if (ArticleRepository != null)
            {
                await ArticleRepository.AddAsync(Model);
            }

            NavigationManager?.NavigateTo("/Articles");
        }
    }
}
