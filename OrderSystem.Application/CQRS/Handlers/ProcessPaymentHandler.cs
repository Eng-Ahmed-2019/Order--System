using MediatR;
using Serilog;
using System.Text.Json;
using OrderSystem.Domain.Entities;
using OrderSystem.Application.Interfaces;
using OrderSystem.Application.CQRS.Commands;

namespace OrderSystem.Application.CQRS.Handlers
{
    public class ProcessPaymentHandler : IRequestHandler<ProcessPaymentCommand, bool>
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IPaymentRepository _paymentRepo;
        private readonly IPaymentLogRepository _logRepo;
        private readonly IOrderRepository _orderRepo;

        public ProcessPaymentHandler(
        IHttpClientFactory httpClientFactory,
        IPaymentRepository paymentRepo,
        IPaymentLogRepository logRepo,
        IOrderRepository orderRepo)
        {
            _httpClientFactory = httpClientFactory;
            _paymentRepo = paymentRepo;
            _logRepo = logRepo;
            _orderRepo = orderRepo;
        }

        public async Task<bool> Handle(ProcessPaymentCommand request, CancellationToken cancellationToken)
        {
            var client = _httpClientFactory.CreateClient("ExternalApi");
            var paymentRequest = new
            {
                orderId = request.orderId,
                amount = request.amount
            };
            var requestJson = JsonSerializer.Serialize(paymentRequest);
            try
            {
                var response = await client.PostAsync(
                        "posts",
                        new StringContent(requestJson, System.Text.Encoding.UTF8, "application/json")
                );
                var responseJson = await response.Content.ReadAsStringAsync();
                await _logRepo.CreateAsync(new PaymentLog
                {
                    RequestJson = requestJson,
                    ResponseJson = responseJson
                });
                await _paymentRepo.CreateAsync(new Payment
                {
                    OrderId = request.orderId,
                    Provider = "JSONPlaceholder",
                    Status = "Success",
                    TransactionId = Guid.NewGuid().ToString()
                });
                return true;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Payment processing failed for OrderId {OrderId}", request.orderId);
                await _logRepo.CreateAsync(new PaymentLog
                {
                    RequestJson = requestJson,
                    ResponseJson = ex.Message
                });
                return false;
            }
        }
    }
}