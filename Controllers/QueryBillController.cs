using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MobileProvider.Services;

namespace MobileProvider.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    public class QueryBillController : ControllerBase
    {
        private readonly BillService _billService;

        public QueryBillController(BillService billService)
        {
            _billService = billService;
        }

        [HttpGet("query")]
        [Authorize]
        public IActionResult QueryBill([FromQuery] string subscriberNo, [FromQuery] int month, [FromQuery] int year)
        {
            try
            {
                var bill = _billService.GetBillSummary(subscriberNo, month, year);
                return Ok(new { TotalAmount = bill.TotalAmount, PaidStatus = bill.PaidStatus });
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
        }
    }
}
