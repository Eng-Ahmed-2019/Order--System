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
            var r = await _mediator.Send(new ProcessPaymentCommand(process.OrderId, process.Amount));
            return r ? Ok("Success Process") : BadRequest("Failed Process");
        }

        [HttpPost("process-stripe")]
        public async Task<IActionResult> ProcessStripe(ProcessPaymentRequestDto dto)
        {
            var result = await _mediator.Send(new ProcessStripePaymentCommand(dto.OrderId, dto.Amount));
            return result ? Ok("Success Process") : BadRequest("Failed Process");
        }
    }
}