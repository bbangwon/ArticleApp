namespace BWShared
{
    public class BWPagerModel
    {
        /*

    public async Task<List<Machine>> GetMachinesPageAsync(int pageIndex, int pageSize)
    {
        return await context.Machines!
                .OrderByDescending(m => m.Id)
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .ToListAsync();
    }

    public Task<int> GetTotalRecordsCountAsync()
    {
        return context.Machines!.CountAsync();
    }


    <Pager PagerModel="pagerModel" PageIndexChanged="OnPageIndexChanged"></Pager>

    @code {
        private PagerModel pagerModel = new PagerModel
        {
            Number = 1,
            Size = 2,
            ButtonCount = 1
        };

        public List<Model>? _models { get; set; }

        protected override async Task OnInitializedAsync()
        {        
            pagerModel.RecordCount = await ModelRepository.GetTotalRecordsCountAsync();
            LoadPages();
        }

        void OnPageIndexChanged(int pageNumber)
        {
            pagerModel.Number = pageNumber;
            LoadPages();        
        }

        async void LoadPages()
        {
            _models = await ModelRepository.GetPageAsync(pagerModel.Index, pagerModel.Size);
            StateHasChanged();
        }
    }
    */


        public int Number { get; set; } = 1;
        public int Index => Number - 1;

        /// <summary>
        /// 한 페이지에 몇개씩 보여줄래
        /// </summary>
        public int Size { get; set; } = 10;

        /// <summary>
        /// 페이지당 버튼은 몇개씩
        /// </summary>
        public int ButtonCount { get; set; } = 3;

        /// <summary>
        /// 총 레코드 개수
        /// </summary>
        public int RecordCount { get; set; } = 0;

        /// <summary>
        /// 총 페이지 개수
        /// </summary>
        public int PageCount
        {
            get
            {
                return Convert.ToInt32(Math.Ceiling(RecordCount / (double)Size));
            }
        }

        /// <summary>
        /// 페이지 시작번호
        /// </summary>
        public int StartNumber
        {
            get
            {
                return ((Convert.ToInt32(Math.Ceiling(Number / (double)ButtonCount)) - 1) * ButtonCount) + 1;
            }
        }

        /// <summary>
        /// 페이지 종료번호
        /// </summary>
        public int EndNumber
        {
            get
            {
                return Math.Min(StartNumber + ButtonCount - 1, PageCount);
            }
        }

        /// <summary>
        /// 이전 페이지 버튼들을 보여줄수 있는가
        /// </summary>
        public bool Previousable
        {
            get
            {
                return StartNumber > 1;
            }
        }

        /// <summary>
        /// 다음 페이지 버튼들을 보여줄수 있는가
        /// </summary>
        public bool Nextable
        {
            get
            {
                return EndNumber < PageCount;
            }
        }
    }        
}
