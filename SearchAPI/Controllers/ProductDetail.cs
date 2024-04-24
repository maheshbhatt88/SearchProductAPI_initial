using Domain;
using MediatR;
using Microsoft.AspNetCore.Mvc;
 using ApplicationLayer;
using Swashbuckle.AspNetCore.Annotations;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SearchAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("Search")]
        [SwaggerOperation(
        Summary = "Search Products API",
        Description = "Searches for products based on specified criteria."
    )]
        public async Task<IActionResult> SearchProduct([FromQuery] ProductSearchData query)
        {
            var reqObj = new SearchProductQueryModel(query);
            var result = await _mediator.Send(reqObj);

            if (result.SearchResult.Count() == 0)
            {
                return Ok("No Data Found.Please Change search parameter");
            }
            else
            {
                string url = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}{HttpContext.Request.Path}{HttpContext.Request.QueryString}";
                SaveSearchHistoryCommand saveSearchHistoryCommand = new SaveSearchHistoryCommand
                {
                    Query = url,
                    Timestamp = DateTime.Now,
                    UserId = 1
                };


               await _mediator.Send(saveSearchHistoryCommand);

                return Ok(result);

            }

        }

    }
}
