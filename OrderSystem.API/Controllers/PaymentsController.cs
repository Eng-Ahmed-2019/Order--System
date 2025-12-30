using MediatR;
using Microsoft.AspNetCore.Mvc;
using OrderSystem.Application.DTOs;
using Microsoft.AspNetCore.Authorization;
using OrderSystem.Application.CQRS.Commands;

namespace OrderSystem.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/payments")]
    public class PaymentsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PaymentsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("process-payment")]
        public async Task<IActionResult>ProcessPayment(ProcessPaymentRequestDto process)
        {
            if (process.OrderId == 0) return BadRequest($"Not found any order match with {process.OrderId}");
            if (process.Amount == 0) return BadRequest("Amount must be greater than zero");
            var r = await _mediator.Send(new ProcessPaymentCommand(process.OrderId, process.Amount));
            if (!r) return BadRequest("Payment failed");
            return Ok("Payment processed");
        }
    }
}