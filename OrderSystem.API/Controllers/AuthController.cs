using Microsoft.AspNetCore.Mvc;
using OrderSystem.Domain.Entities;
using OrderSystem.Application.DTOs;
using OrderSystem.Application.Services;
using OrderSystem.Application.Interfaces;
using OrderSystem.Application.Validators;

namespace OrderSystem.API.Controllers
{
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly JwtTokenGenerator _jwtTokenGenerator;

        public AuthController(IUserRepository userRepository, JwtTokenGenerator jwtTokenGenerator)
        {
            _userRepository = userRepository;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequestDto registerRequestDto)
        {
            var user = new User
            {
                Email = registerRequestDto.Email,
                FullName = registerRequestDto.FullName,
                NationalId = registerRequestDto.NationalId,
                PasswordHash = PasswordHasher.Hash(registerRequestDto.Password),
            };
            await _userRepository.CreateAsync(user);
            return Ok("User registered Successfully");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequestDto dto)
        {
            var user = await _userRepository.GetByEmailAsync(dto.Email);
            if (user == null) return BadRequest("Invalid email or password");
            if (user.PasswordHash != PasswordHasher.Hash(dto.Password)) return BadRequest("Invalid email or password");
            var result = _jwtTokenGenerator.GenerateToken(user.Id, user.Email);
            return Ok(new LoginResponseDto
            {
                Token = result.token,
                ExpiresAt = result.expiresAt,
            });
        }
    }
}