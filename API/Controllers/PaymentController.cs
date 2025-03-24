using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Wego.Core.Models;
using Wego.Core.Services.Contract;

namespace Wego.API.Controllers
{
    public class PaymentController : BaseApiController
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpPost("process")]
        public async Task<IActionResult> ProcessPayment([FromBody] PaymentRequest request)
        {
            var clientSecret = await _paymentService.ProcessPaymentAsync(request.Amount, request.Currency);
            return Ok(new { ClientSecret = clientSecret });
        }
    }
}
