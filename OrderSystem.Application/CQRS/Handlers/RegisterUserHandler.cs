using MediatR;
using Serilog;
using OrderSystem.Domain.Entities;
using OrderSystem.Application.Interfaces;
using OrderSystem.Application.Validators;
using OrderSystem.Application.CQRS.Commands;

namespace OrderSystem.Application.CQRS.Handlers
{
    public class RegisterUserHandler : IRequestHandler<RegisterUserCommand>
    {
        private readonly IUserRepository _userRepository;

        public RegisterUserHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task Handle(RegisterUserCommand command,CancellationToken cancellation)
        {
            Log.Information(
                "Registering new user with Email {Email}",
                command.Email
            );
            if (command == null) return;
            var user = new User
            {
                FullName = command.FullName,
                Email = command.Email,
                PasswordHash = PasswordHasher.Hash(command.Password),
                NationalId = command.NationaId
            };
            await _userRepository.CreateAsync(user);
            Log.Information(
                "User registered successfully with Email {Email}",
                command.Email
            );
        }
    }
}