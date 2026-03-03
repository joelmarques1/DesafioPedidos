using DesafioPedidos.Domain.Entities;

namespace DesafioPedidos.Domain.Interfaces.Repositories;

public interface IPedidoRepository
{
    Task<Pedido?> BuscarPorNumeroAsync(string numeroPedido, CancellationToken cancellationToken = default);
    Task<IEnumerable<Pedido>> ListarTodosAsync(CancellationToken cancellationToken = default);
    Task AdicionarAsync(Pedido pedido, CancellationToken cancellationToken = default);
    Task AtualizarAsync(Pedido pedido, CancellationToken cancellationToken = default);
    Task RemoverAsync(Pedido pedido, CancellationToken cancellationToken = default);
    Task<bool> ExisteAsync(string numeroPedido, CancellationToken cancellationToken = default);
}
