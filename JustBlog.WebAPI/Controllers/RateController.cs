using JustBlog.Application.Rates;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JustBlog.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class RateController : ControllerBase
    {
        private readonly IRateService rateService;

        public RateController(IRateService rateService)
        {
            this.rateService = rateService;
        }

        [HttpPost]
        public IActionResult ChangeVote(int userId, int postId)
        {
            rateService.ChangeRate(userId, postId);
            return NoContent();
        }
    }
}