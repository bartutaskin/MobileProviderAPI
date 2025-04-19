using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MobileProvider.Services;

namespace SE4453_MobileProvider.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    public class QueryBillDetailedController : ControllerBase
    {
        private readonly BillService _billService;

        public QueryBillDetailedController(BillService billService)
        {
            _billService = billService;
        }

        [HttpGet("query-detailed")]
        [Authorize]
        public IActionResult QueryBillDetails([FromQuery] string subscriberNo, [FromQuery] int year, [FromQuery] int page = 1, [FromQuery] int pageSize = 5) {
            try
            {
                var bills = _billService.GetBillDetails(subscriberNo, year, page, pageSize);
                return Ok(bills);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
        }   
    }
}
