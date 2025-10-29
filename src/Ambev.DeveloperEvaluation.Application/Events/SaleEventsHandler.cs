using Ambev.DeveloperEvaluation.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Events
{
    public class SaleEventsHandler :
        INotificationHandler<SaleCreatedEvent>,
        INotificationHandler<SaleCancelledEvent>
    {
        private readonly ILogger<SaleEventsHandler> _logger;

        public SaleEventsHandler(ILogger<SaleEventsHandler> logger)
        {
            _logger = logger;
        }

        public Task Handle(SaleCreatedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation(
                "Venda criada: {SaleNumber} com valor total {TotalAmount}",
                notification.SaleNumber,
                notification.TotalAmount
            );
            return Task.CompletedTask;
        }

        public Task Handle(SaleCancelledEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogWarning(
                "Venda cancelada: {SaleNumber}",
                notification.SaleNumber
            );
            return Task.CompletedTask;
        }
    }
}