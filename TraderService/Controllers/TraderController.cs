using Microsoft.AspNetCore.Mvc;
using RepoInterfaceLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using TraderModelLib;
using TraderModelLib.Data;
using TraderModelLib.Models;

namespace TraderService.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TraderController : Controller
    {
        private readonly IRepo<TraderDbContext> _repo;

        public TraderController(IRepo<TraderDbContext> repo)
        {
            _repo = repo;
        }

        [HttpGet]
        [Route("all")]
        public async Task<IActionResult> GetAsync()
        {
            try
            {
                return Ok(await _repo.FetchAsync(dbContext => dbContext.Traders?.ToList()));
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetByIfAsync(int id)
        {
            try
            {
                return Ok(await _repo.FetchAsync(dbContext => dbContext.Traders?.Where(t => t.Id == id).FirstOrDefault()));
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet]
        [Route("delete/{id}")]
        public async Task<RepoResponse> DeleteAsync(int id)
        {
            Trader trader;
            RepoResponse repoResponse = new() { OpStatus = RepoOperationStatus.Failure };
            try
            {
                trader = await _repo.FetchAsync(dbContext => dbContext.Traders?.Where(t => t.Id == id).FirstOrDefault());
            }
            catch (Exception e)
            {
                repoResponse.Message = e.Message;
                return repoResponse;
            }

            if (trader != null)
            {
                trader.IsDeleted = true;
                repoResponse = await _repo.SaveAsync(dbContext => dbContext.Traders.Update(trader));
            }

            return repoResponse;
        }

        [HttpPost]
        [Route("add")]
        public RepoResponse Add([FromBody] JsonElement arg)
        {
            //var json = element.GetRawText();
            //return JsonSerializer.Deserialize<T>(json);

            Trader trader;
            RepoResponse repoResponse = new() { OpStatus = RepoOperationStatus.Failure };
            //try
            //{
            //    trader = await _repo.FetchAsync(dbContext => dbContext.Traders?.Where(t => t.Id == id).FirstOrDefault());
            //}
            //catch (Exception e)
            //{
            //    repoResponse.Message = e.Message;
            //    return repoResponse;
            //}

            //if (trader != null)
            //{
            //    trader.IsDeleted = true;
            //    repoResponse = await _repo.SaveAsync(dbContext => dbContext.Traders.Update(trader));
            //}

            return repoResponse;
        }
    }
}
