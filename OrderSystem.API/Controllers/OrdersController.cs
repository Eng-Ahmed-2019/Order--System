using MediatR;
using Microsoft.AspNetCore.Mvc;
using OrderSystem.Application.DTOs;
using OrderSystem.Application.CQRS.Queries;
using OrderSystem.Application.CQRS.Commands;

namespace OrderSystem.API.Controllers
{
    public class OrdersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OrdersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("create-order")]
        public async Task<IActionResult>CreateOrder(CreateOrderRequestDto dto)
        {
            var id = await _mediator.Send(new CreateOrderCommand(dto));
            return Ok(new { OrderId = id });
        }

        [HttpGet("get-order/{id}")]
        public async Task<IActionResult> GetOrder(int id)
        {
            var order = await _mediator.Send(new GetOrderByIdQuery(id));
            if (order == null) return NotFound($"Not found order match with: {id}");
            return Ok(order);
        }
    }
}