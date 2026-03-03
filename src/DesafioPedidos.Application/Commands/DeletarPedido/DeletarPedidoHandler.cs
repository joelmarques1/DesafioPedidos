using DesafioPedidos.Domain.Exceptions;
using DesafioPedidos.Domain.Interfaces.Repositories;

namespace DesafioPedidos.Application.Commands.DeletarPedido;

public interface IDeletarPedidoHandler
{
    Task HandleAsync(DeletarPedidoCommand command, CancellationToken cancellationToken = default);
}

public class DeletarPedidoHandler : IDeletarPedidoHandler
{
    private readonly IPedidoRepository _repository;

    public DeletarPedidoHandler(IPedidoRepository repository)
    {
        _repository = repository;
    }

    public async Task HandleAsync(DeletarPedidoCommand command, CancellationToken cancellationToken = default)
    {
        var pedido = await _repository.BuscarPorNumeroAsync(command.NumeroPedido, cancellationToken)
            ?? throw new PedidoNaoEncontradoException(command.NumeroPedido);

        await _repository.RemoverAsync(pedido, cancellationToken);
    }
}
