using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Ambev.DeveloperEvaluation.Domain.Enums;

namespace Ambev.DeveloperEvaluation.Domain.Entities
{
    public class Sale : BaseEntity
    {
        public string SaleNumber { get; private set; } = null!;

        public Guid ProductId { get; private set; }

        public Product Product { get; private set; } = null!;

        public int Quantity { get; private set; }

        public decimal UnitPrice { get; private set; }

        public decimal DiscountPercentage { get; private set; }

        public decimal DiscountAmount => (Quantity * UnitPrice) * (DiscountPercentage / 100);

        public decimal TotalAmount => (Quantity * UnitPrice) - DiscountAmount;

        public decimal ItemTotalAmount => Quantity * UnitPrice;

        public DateTime SaleDate { get; private set; }

        public Guid CustomerId { get; private set; }

        public User Customer { get; private set; } = null!;

        public Guid SellerId { get; private set; }

        public User Seller { get; private set; } = null!;

        public Guid BranchId { get; private set; }

        public string BranchName { get; private set; } = null!;

        public SaleStatus Status { get; private set; } = SaleStatus.Completed;

        public Sale(Guid productId, int quantity, decimal unitPrice, Guid customerId, Guid sellerId, Guid branchId, string branchName)
        {
            Id = Guid.NewGuid();
            SaleNumber = GenerateSaleNumber();
            ProductId = productId;
            SetQuantity(quantity);
            SetUnitPrice(unitPrice);
            CustomerId = customerId;
            SellerId = sellerId;
            BranchId = branchId;
            BranchName = branchName;
            SaleDate = DateTime.UtcNow;
            CalculateDiscount();

            AddDomainEvent(new SaleCreatedEvent(Id, SaleNumber, TotalAmount));
        }

        protected Sale() { }

        public void SetQuantity(int quantity)
        {
            if (quantity <= 0)
                throw new DomainException("A quantidade deve ser maior que zero");

            if (quantity > 20)
                throw new DomainException("Não é possível vender mais de 20 itens idênticos.");

            Quantity = quantity;
            CalculateDiscount();
        }

        public void SetUnitPrice(decimal unitPrice)
        {
            if (unitPrice <= 0)
                throw new DomainException("O preço unitário deve ser maior que zero");

            UnitPrice = unitPrice;
        }

        public void UpdateSale(int quantity, decimal unitPrice)
        {
            SetQuantity(quantity);
            SetUnitPrice(unitPrice);
            CalculateDiscount();

            AddDomainEvent(new SaleModifiedEvent(Id, SaleNumber, TotalAmount));
        }

        public void Cancel()
        {
            if (Status == SaleStatus.Cancelled)
                throw new DomainException("A venda já foi cancelada");

            Status = SaleStatus.Cancelled;
            AddDomainEvent(new SaleCancelledEvent(Id, SaleNumber));
        }

        private void CalculateDiscount()
        {
            if (Quantity >= 10 && Quantity <= 20)
            {
                DiscountPercentage = 20m;
            }
            else if (Quantity >= 4)
            {
                DiscountPercentage = 10m;
            }
            else
            {
                DiscountPercentage = 0m;
            }
        }

        private static string GenerateSaleNumber()
        {
            return $"SALE-{DateTime.UtcNow:yyyyMMdd-HHmmss}-{Guid.NewGuid().ToString("N")[..8].ToUpper()}";
        }
    }
}