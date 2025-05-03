using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BuildingBlocks.CQRS
{

	public interface ICommandHandler<in TCommand>
		: MediatR.IRequestHandler<TCommand, Unit>
		where TCommand : ICommand<Unit>
	{
	}
	public interface IRequestHandler<in TCommand, TResponse>
		: MediatR.IRequestHandler<TCommand, TResponse>
		where TCommand : ICommand<TResponse>
		where TResponse : notnull
	{
	}
}
