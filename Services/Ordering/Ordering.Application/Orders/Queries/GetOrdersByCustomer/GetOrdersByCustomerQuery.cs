using MediatR;
using Microsoft.EntityFrameworkCore;
using Ordering.Application.Data;
using Ordering.Application.Dtos;
using Ordering.Application.Extensions;
using Ordering.Domain.ValueObjects;

namespace Ordering.Application.Orders.Queries.GetOrdersByCustomer;

public record GetOrdersByCustomerQuery(Guid CustomerId)
	: IRequest<GetOrdersByCustomerResult>;

public record GetOrdersByCustomerResult(IEnumerable<OrderDto> Orders);


	public class GetOrdersByCustomerQueryHandler(IApplicationDBContext dbContext)
		: IRequestHandler<GetOrdersByCustomerQuery, GetOrdersByCustomerResult>
	{
		public async Task<GetOrdersByCustomerResult> Handle(GetOrdersByCustomerQuery query, CancellationToken cancellationToken)
		{
			// get orders by customer using dbContext
			// return result

			var orders = await dbContext.Orders
							.Include(o => o.OrderItems)
							.AsNoTracking()
							.Where(o => o.CustomerId == CustomerId.Of(query.CustomerId))
							.OrderBy(o => o.OrderName.Value)
							.ToListAsync(cancellationToken);

			return new GetOrdersByCustomerResult(orders.ToOrderDtoList());
		}
	}

