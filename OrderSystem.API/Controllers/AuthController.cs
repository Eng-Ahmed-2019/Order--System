using MediatR;
using Microsoft.AspNetCore.Mvc;
using OrderSystem.Application.DTOs;
using Microsoft.AspNetCore.Authorization;
using OrderSystem.Application.CQRS.Commands;

namespace OrderSystem.API.Controllers
{
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequestDto dto)
        {
            await _mediator.Send(new RegisterUserCommand(
                dto.FullName,
                dto.NationalId,
                dto.Password,
                dto.Email
            ));
            return Ok("User Registered Successfully");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequestDto dto)
        {
            var r = await _mediator.Send(new LoginUserCommand(
                dto.Email,
                dto.Password
            ));
            return Ok(r);
        }

        [Authorize]
        [HttpPost("log-out")]
        public async Task<IActionResult> LogOut()
        {
            var sid = User.FindFirst("sid")!.Value;
            await _mediator.Send(new LogoutUserCommand(Guid.Parse(sid)));
            return Ok("Logged out successfully");
        }
    }
}