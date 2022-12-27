using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace NoticeApp.Models.Tests
{
    [TestClass]
    public class NoticeRepositoryTest
    {
        [TestMethod]
        public async Task NoticeRepositoryAllMethodTest()
        {
            #region 준비
            var options = new DbContextOptionsBuilder<NoticeAppDbContext>()
            .UseInMemoryDatabase(databaseName: $"NoticeApp{Guid.NewGuid()}").Options;

            var serviceProvider = new ServiceCollection().AddLogging().BuildServiceProvider();
            var factory = serviceProvider.GetService<ILoggerFactory>();
            #endregion

            #region AddAsync 테스트
            using (var context = new NoticeAppDbContext(options))
            {
                var repository = new NoticeRepository(context, factory!);
                var model = new Notice
                {
                    Name = "관리자",
                    Title = "공지사항입니다.",
                    Content = "내용입니다."
                };

                await repository.AddAsync(model);
            }
            using (var context = new NoticeAppDbContext(options))
            {
                Assert.AreEqual(1, await context.Notices.CountAsync());
                var model = await context.Notices.Where(n => n.Id == 1).SingleOrDefaultAsync();

                Assert.AreEqual("관리자", model!.Name);
            }
            #endregion

            #region GetAllAsync 테스트
            using (var context = new NoticeAppDbContext(options))
            {
                var repository = new NoticeRepository(context, factory!);
                var model = new Notice
                {
                    Name = "홍길동",
                    Title = "공지사항입니다.",
                    Content = "내용입니다."
                };

                await repository.AddAsync(model);
                await repository.AddAsync(new Notice
                {
                    Name = "백두산",
                    Title = "공지사항입니다."
                });

            }
            using (var context = new NoticeAppDbContext(options))
            {
                var repository = new NoticeRepository(context, factory!);
                var models = await repository.GetAllAsync();

                Assert.AreEqual(3, models.Count);
            } 
            #endregion

            using(var context = new NoticeAppDbContext(options))
            {
                int parentId = 1;

                var no1 = await context.Notices.Where(m => m.Id == 1).SingleOrDefaultAsync();

                no1!.ParentId = parentId;
                no1!.IsPinned = true;

                context.Update(no1);
                context.SaveChanges();

                var repository = new NoticeRepository(context, factory!);
                var r = await repository.GetTotalRecords(parentId);
                var p = await repository.GetPinnedRecords(parentId);

                Assert.AreEqual(1, r);
                Assert.AreEqual(1, p);
            }
        }
    }
}
