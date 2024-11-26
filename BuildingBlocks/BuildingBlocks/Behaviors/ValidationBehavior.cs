﻿using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingBlocks.Behaviors
{
	public class ValidationBehavior<TRequest, TResponse> (IEnumerable<IValidator<TRequest>> validators)
		: IPipelineBehavior<TRequest, TResponse>
		where TRequest : IRequest<TResponse>
	{
		public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
		{
			var context = new ValidationContext<TRequest>(request);
			var validationresult = await Task.WhenAll(
				validators.
				Select(x => x.ValidateAsync(context,cancellationToken))
				);
			var failures = validationresult.Where(x => x.Errors.Any())
				.SelectMany(e => e.Errors).ToList();

			if (failures.Any())
			{
				throw new ValidationException(failures);
			}
			return await next();
		}
	}
}
