using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Domain.Events
{
    public class SaleCreatedEvent : DomainEvent
    {
        public Guid SaleId { get; }

        public string SaleNumber { get; }

        public decimal TotalAmount { get; }

        public SaleCreatedEvent(Guid saleId, string saleNumber, decimal totalAmount)
        {
            SaleId = saleId;
            SaleNumber = saleNumber;
            TotalAmount = totalAmount;
        }
    }
}