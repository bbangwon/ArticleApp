using ArticleApp.Models;
using BWPager;
using Microsoft.AspNetCore.Components;

namespace ArticleApp.Pages.Articles
{
    public partial class Index
    {
        [Inject]
        IArticleRepository? ArticleRepository { get; set; }

        [Inject]
        NavigationManager? NavigationManager { get; set; }

        private List<Article>? articles;

        private BWPagerModel pagerModel = new BWPagerModel()
        {
            Size = 2,
            ButtonCount = 5
        };

        protected override async Task OnInitializedAsync()
        {
            if(ArticleRepository != null)
            {
                pagerModel.RecordCount = await ArticleRepository.GetTotalRecordsCountAsync();
            }
            LoadPages();
        }

        async void LoadPages()
        {
            if(ArticleRepository != null )
            {
                articles = await ArticleRepository.GetPageAsync(pagerModel.Index, pagerModel.Size);
            }
            StateHasChanged();
        }

        void OnPageIndexChanged(int pageNumber)
        {
            LoadPages();
        }

        private void btnTitle_Click(int id)
        {
            NavigationManager?.NavigateTo($"/Articles/Details/{id}");
        }
    }
}
