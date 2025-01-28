using BuildingBlocks.CQRS;
using BuildingBlocks.Pagination;
using Microsoft.EntityFrameworkCore;
using Ordering.Application.Data;
using Ordering.Application.Dtos;
using Ordering.Application.Extensions;

namespace Ordering.Application.Orders.Queries.GetOrders
{
    public class GetOrdersHandler(IApplicationDbContext _context) : IQueryHandler<GetOrdersQuery, GetOrdersResult>
    {
        public async Task<GetOrdersResult> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
        {
            var pageIndex = request.PaginationRequest.PageIndex;
            var pageSize = request.PaginationRequest.PageSize;

            var totalCount = await _context.Orders.LongCountAsync(cancellationToken);
            var orders = await _context.Orders.Include(o => o.OrderItems)
                .OrderBy(o => o.OrderName.Value)
                .Skip(pageSize * pageIndex)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return new GetOrdersResult(new PaginatedResult<OrderDto>(pageIndex, pageSize, totalCount, orders.ToOrderDtoList()));
        }
    }
}
