using ArticleApp.Models.Articles;
using Microsoft.EntityFrameworkCore;

namespace ArticleApp.Models.Tests
{
    [TestClass]
    public class ArticleRepositoryTest
    {
        [TestMethod]
        public async Task ArticleRepositoryAllMethodTest()
        {
            //AddAsync
            var options = new DbContextOptionsBuilder<ArticleAppDbContext>()
                .UseInMemoryDatabase(databaseName: "ArticleApp").Options;
                //.UseSqlServer("server=(localdb)\\mssqllocaldb;database=ArticleApp;Integrated Security=True;").Options;            

            using (var context = new ArticleAppDbContext(options))
            {
                var repository = new ArticleRepository(context);
                var model = new Article
                {                  
                    Title= "[1] 게시판 시작",
                    Created= DateTime.Now,
                };
                await repository.AddAsync(model);
                await context.SaveChangesAsync();
            }

            using (var context = new ArticleAppDbContext(options))
            {
                Assert.AreEqual(1, context.Articles.Count());

                var model = context.Articles.Where(m => m.Id== 1).SingleOrDefault();
                Assert.AreEqual("[1] 게시판 시작", model?.Title);
            }

            //GetAllAsync
            using (var context = new ArticleAppDbContext(options))
            {
                //트랜잭션 관련 코드는 InMemoryDatabase 공급자에서는 지원 X
                // using (var transaction = context.Database.BeginTransaction()) { transaction.Commit(); }

                var model = new Article
                {
                    Title = "[2] 게시판 가동",
                    Created = DateTime.Now,
                };
                await context.Articles.AddAsync(model);
                await context.SaveChangesAsync();

                await context.Articles.AddAsync(new Article
                {
                    Title = "[3] 게시판 중지",
                    Created = DateTime.Now,
                });
                await context.SaveChangesAsync();
            }

            using (var context = new ArticleAppDbContext(options))
            {
                var repository = new ArticleRepository(context);
                var models = await repository.GetAllAsync();

                Assert.AreEqual(3, models.Count);
            }


            //GetById
            using (var context = new ArticleAppDbContext(options))
            {
                //Empty
            }

            using (var context = new ArticleAppDbContext(options))
            {
                var repository = new ArticleRepository(context);
                var model = await repository.GetByIdAsync(2);

                Assert.IsTrue(model?.Title?.Contains("가동"));
                Assert.AreEqual("[2] 게시판 가동", model?.Title);
            }

            //Update
            using (var context = new ArticleAppDbContext(options))
            {
                //Empty
            }

            using (var context = new ArticleAppDbContext(options))
            {
                var repository = new ArticleRepository(context);
                var model = await repository.GetByIdAsync(2);
                if (model == null)
                    throw new AssertFailedException();

                model.Title = "[2] 게시판 바쁨";
                await repository.UpdateAsync(model);
                await context.SaveChangesAsync();


                Assert.AreEqual("[2] 게시판 바쁨", (await context.Articles.Where(m => m.Id==2).SingleOrDefaultAsync())?.Title);
            }

            //Delete
            using (var context = new ArticleAppDbContext(options))
            {
                //Empty
            }

            using (var context = new ArticleAppDbContext(options))
            {
                var repository = new ArticleRepository(context);
                await repository.DeleteAsync(2);
                await context.SaveChangesAsync();

                Assert.AreEqual(2, await context.Articles.CountAsync());
                Assert.IsNull(await repository.GetByIdAsync(2));
            }

            //PagingAsync()
            using (var context = new ArticleAppDbContext(options))
            {
                //Empty
            }

            using (var context = new ArticleAppDbContext(options))
            {
                int pageIndex = 0;
                int pageSize = 1;

                var repository = new ArticleRepository(context);
                var models = await repository.GetPageAsync(pageIndex, pageSize);
                var totalRecord = await repository.GetTotalRecordsCountAsync();

                Assert.AreEqual("[3] 게시판 중지", models?.FirstOrDefault()?.Title);
                Assert.AreEqual(2, totalRecord);
            }
        }
    }
}
