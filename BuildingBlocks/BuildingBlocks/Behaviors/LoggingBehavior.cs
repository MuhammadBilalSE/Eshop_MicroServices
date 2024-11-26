using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingBlocks.Behaviors
{
	public class LoggingBehavior<TRequest, TResponse>(ILogger<LoggingBehavior<TRequest,TResponse>> logger) :
		IPipelineBehavior<TRequest, TResponse>
		where TRequest : notnull, IRequest<TResponse>
		where TResponse : notnull
	{
		public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
		{
			logger.LogInformation($"[START] Handle Request ={typeof(TRequest).Name} - Response ={typeof(TResponse).Name}");
			var timer = new Stopwatch();
			timer.Start();
			var resposne = await next();
			timer.Stop();
			var timeTaken = timer.Elapsed;
			if (timeTaken.Seconds > 3) // Took More than 3 Seconds to Perform the Action
			{
				logger.LogWarning($"[PERFORMANCE] The Request {typeof(TRequest).Name} took {timeTaken} ");
			}

			return resposne;
		}
	}
}
