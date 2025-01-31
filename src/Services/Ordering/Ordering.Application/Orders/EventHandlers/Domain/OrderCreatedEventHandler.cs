using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.FeatureManagement;
using Ordering.Application.Extensions;
using Ordering.Domain.Events;

namespace Ordering.Application.Orders.EventHandlers.Domain
{
    public class OrderCreatedEventHandler(IPublishEndpoint publishEndpoint, ILogger<OrderCreatedEventHandler> _logger, IFeatureManager featureManager) : INotificationHandler<OrderCreatedEvent>
    {
        public async Task Handle(OrderCreatedEvent domainEvent, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Domain Event handled: {DomainEvent}", domainEvent.GetType().Name);
            
            if(await featureManager.IsEnabledAsync("OrderFullfilment"))
            {
                var orderCreatedIntegrationEvent = domainEvent.order.ToOrderDto();
                await publishEndpoint.Publish(orderCreatedIntegrationEvent, cancellationToken);
            } 
        }
    }
}
