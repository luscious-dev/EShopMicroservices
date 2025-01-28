using BuildingBlocks.CQRS;
using Ordering.Application.Data;
using Ordering.Application.Exceptions;
using Ordering.Domain.ValueObjects;

namespace Ordering.Application.Orders.Commands.DeleteOrder
{
    internal class DeleteOrderHandler(IApplicationDbContext _context) : ICommandHandler<DeleteOrderCommand, DeleteOrderResult>
    {
        public async Task<DeleteOrderResult> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
        {
            var order = await _context.Orders.FindAsync([OrderId.Of(request.OrderId)], cancellationToken: cancellationToken);

            if(order == null)
                throw new OrderNotFoundException(request.OrderId);
            _context.Orders.Remove(order);
            
            await _context.SaveChangesAsync(cancellationToken);

            return new DeleteOrderResult(true);
        }
    }
}
