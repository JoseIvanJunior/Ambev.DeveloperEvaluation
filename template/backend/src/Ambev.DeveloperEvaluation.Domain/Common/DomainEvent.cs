using MediatR;
using System;

namespace Ambev.DeveloperEvaluation.Domain.Common
{
    public abstract class DomainEvent : INotification
    {
        public DateTime OccurredOn { get; protected set; } = DateTime.UtcNow;

        public Guid EventId { get; protected set; } = Guid.NewGuid();
    }
}