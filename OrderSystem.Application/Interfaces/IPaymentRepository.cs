using OrderSystem.Domain.Entities;

namespace OrderSystem.Application.Interfaces
{
    public interface IPaymentRepository
    {
        Task<int> CreateAsync(Payment payment);
    }
}