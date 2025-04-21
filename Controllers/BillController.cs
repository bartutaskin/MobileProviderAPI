using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MobileProvider.Models.DTOs;
using MobileProvider.Services;
using MobileProvider.Models.DTOs;

namespace MobileProvider.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]

    public class BillController : ControllerBase
    {
        private readonly BillService _billService;

        public BillController(BillService billService)
        {
            _billService = billService;
        }

        [HttpPost("calculate")]
        [Authorize]
        public IActionResult CalculateBill([FromBody] CalculateBillDto dto)
        {
            try
            {
                var billAmount = _billService.CalculateBill(dto);
                return Ok(new { BillAmount = billAmount });
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
        }

        [HttpPost("pay")]
        public async Task<IActionResult> PayBill([FromBody] PayBillDto dto)
        {
            try
            {
                var result = await _billService.PayBillAsync(dto);
                return Ok(new { Message = result });
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
        }
    }
}
