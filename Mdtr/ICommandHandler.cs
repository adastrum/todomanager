using System.Threading;
using System.Threading.Tasks;

namespace Mdtr
{
    public interface ICommandHandler<TCommand>
        where TCommand : ICommand
    {
        Task Handle(TCommand command, CancellationToken cancellationToken = default);
    }
}
