using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Domain.Events
{
    public class ItemCancelledEvent : DomainEvent
    {
        public Guid SaleId { get; }

        public Guid ProductId { get; }

        public int Quantity { get; }

        public ItemCancelledEvent(Guid saleId, Guid productId, int quantity)
        {
            SaleId = saleId;
            ProductId = productId;
            Quantity = quantity;
        }
    }
}