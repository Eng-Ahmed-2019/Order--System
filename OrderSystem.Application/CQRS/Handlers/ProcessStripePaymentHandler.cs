using MediatR;
using Serilog;
using System.Text.Json;
using System.Net.Http.Headers;
using OrderSystem.Domain.Enums;
using OrderSystem.Domain.Entities;
using OrderSystem.Application.Interfaces;
using OrderSystem.Application.CQRS.Commands;

namespace OrderSystem.Application.CQRS.Handlers
{
    public class ProcessStripePaymentHandler
         : IRequestHandler<ProcessStripePaymentCommand, bool>
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IPaymentLogRepository _logRepository;
        private readonly IPaymentRepository _paymentRepo;

        public ProcessStripePaymentHandler(
            IHttpClientFactory httpClientFactory,
            IPaymentLogRepository logRepository,
            IPaymentRepository paymentRepo
            )
        {
            _httpClientFactory = httpClientFactory;
            _logRepository = logRepository;
            _paymentRepo = paymentRepo;
        }

        public async Task<bool> Handle(ProcessStripePaymentCommand command, CancellationToken cancellationToken)
        {
            Log.Information(
                "Starting Stripe payment for OrderId {OrderId}",
                command.orderId
            );
            if (command == null) return false;
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri("https://api.stripe.com/v1/");
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(
                    "Bearer",
                    "sk_test_XXXXXXXXXXXXXXXXXXXX"
                );
            var paymentData = new Dictionary<string, string>
            {
                { "amount", ((int)(command.amount * 100)).ToString() },
                {"currency","Usd" },
                {"payment_method_types[]","Card" }
            };
            var content = new FormUrlEncodedContent(paymentData);
            try
            {
                var response = await client.PostAsync(
                "payment_intents",
                content,
                cancellationToken
                );
                var responseJson = await response.Content.ReadAsStringAsync();
                await _logRepository.CreateAsync(
                    new PaymentLog
                    {
                        RequestJson = JsonSerializer.Serialize(paymentData),
                        ResponseJson = responseJson
                    }
                );
                await _paymentRepo.CreateAsync(
                    new Payment
                    {
                        OrderId = command.orderId,
                        Provider = "Stripe",
                        Status = response.IsSuccessStatusCode
                        ? PaymentStatus.Success
                        : PaymentStatus.Failed,
                        TransactionId = Guid.NewGuid().ToString()
                    }
                );
                Log.Information(
                    "Stripe payment finished for OrderId {OrderId} with Status {Status}",
                    command.orderId,
                    response.IsSuccessStatusCode
                );
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Log.Error(ex,
                    "Stripe payment failed for OrderId {OrderId}",
                    command.orderId
                );
                await _logRepository.CreateAsync(
                    new PaymentLog
                    {
                        RequestJson = JsonSerializer.Serialize(paymentData),
                        ResponseJson = ex.Message
                    }
                );
                return false;
            }
        }
    }
}