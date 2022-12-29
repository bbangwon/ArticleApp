using Microsoft.AspNetCore.Mvc;
using NoticeApp.Models;
using System.Runtime.InteropServices;

namespace NoticeApp.Apis.Controllers
{
    [Produces("application/json")]
    [Route("api/Notices")]
    public class NoticeController : ControllerBase
    {
        private readonly INoticeRepository repository;
        private readonly ILogger logger;

        public NoticeController(INoticeRepository repository, ILoggerFactory loggerFactory)
        {
            this.repository = repository;
            this.logger = loggerFactory.CreateLogger<NoticeController>();
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var notices = await repository.GetAllAsync();
                return Ok(notices);
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                return BadRequest();                
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddAsync([FromBody]Notice model)
        {
            var tmpModel = new Notice
            {
                Name = model.Name,
                Title = model.Title,
                Content = model.Content,
                ParentId = model.ParentId,
                Created = DateTime.Now
            };

            if(!ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                var newModel = await repository.AddAsync(tmpModel);
                if(newModel == null)
                {
                    return BadRequest();
                }
                var uri = Url.Link("GetNoticeById", new { id = newModel.Id });
                return Created(uri!, newModel);                
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                return BadRequest();
            }
        }

        [HttpGet("{id}", Name = "GetNoticeById")]
        public async Task<IActionResult> GetById([FromRoute]int id)
        {
            try
            {
                var model = await repository.GetByIdAsync(id);
                return Ok(model);
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                return BadRequest();
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody]Notice model)
        {
            model.Id = id;

            if(!ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                var status = await repository.UpdateAsync(model);
                if(!status)
                {
                    return BadRequest();
                }
                return Ok();
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                return BadRequest();
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            try
            {
                var status = await repository.RemoveAsync(id);
                if(!status)
                {
                    return BadRequest();
                }
                return Ok();
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                return BadRequest();
            }
        }

        [HttpGet("page")]
        public async Task<IActionResult> GetAll([FromQuery] int pageIndex, [FromQuery] int pageSize)
        {
            try
            {
                var recordCount = await repository.GetTotalRecords();
                var models = await repository.GetAllOfPageAsync(pageIndex, pageSize);

                Response.Headers.Add("X-TotalRecordCount", recordCount.ToString());
                Response.Headers.Add("Access-Control-Expose-Headers", "X-TotalRecordCount");

                return Ok(models);
            }
            catch(Exception e)
            {
                logger.LogError(e.Message);
                return BadRequest();
            }
        }
    }
}
