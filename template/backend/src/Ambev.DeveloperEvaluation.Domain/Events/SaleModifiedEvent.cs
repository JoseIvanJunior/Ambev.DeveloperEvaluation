using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Domain.Events
{
    public class SaleModifiedEvent : DomainEvent
    {
        public Guid SaleId { get; }

        public string SaleNumber { get; }

        public decimal NewTotalAmount { get; }

        public SaleModifiedEvent(Guid saleId, string saleNumber, decimal newTotalAmount)
        {
            SaleId = saleId;
            SaleNumber = saleNumber;
            NewTotalAmount = newTotalAmount;
        }
    }
}