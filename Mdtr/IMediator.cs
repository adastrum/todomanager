using System.Threading;
using System.Threading.Tasks;

namespace Mdtr
{
    public interface IMediator
    {
        Task SendCommand<TCommand>(TCommand command, CancellationToken cancellationToken = default)
            where TCommand : ICommand;

        Task<TResult> SendQuery<TQuery, TResult>(TQuery query, CancellationToken cancellationToken = default)
            where TQuery : IQuery<TResult>;
    }
}
