using DesafioPedidos.Application.DTOs;
using DesafioPedidos.Domain.Interfaces.Repositories;

namespace DesafioPedidos.Application.Queries.ListarPedidos;

public record ListarPedidosQuery();

public interface IListarPedidosHandler
{
    Task<IEnumerable<PedidoResponseDto>> HandleAsync(ListarPedidosQuery query, CancellationToken cancellationToken = default);
}

public class ListarPedidosHandler : IListarPedidosHandler
{
    private readonly IPedidoRepository _repository;

    public ListarPedidosHandler(IPedidoRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<PedidoResponseDto>> HandleAsync(ListarPedidosQuery query, CancellationToken cancellationToken = default)
    {
        var pedidos = await _repository.ListarTodosAsync(cancellationToken);

        return pedidos.Select(pedido => new PedidoResponseDto
        {
            Pedido = pedido.NumeroPedido,
            Itens = pedido.Itens.Select(i => new ItemDto
            {
                Descricao = i.Descricao,
                PrecoUnitario = i.PrecoUnitario,
                Qtd = i.Qtd
            }).ToList(),
            ValorTotal = pedido.ValorTotal,
            QuantidadeTotalItens = pedido.QuantidadeTotalItens
        });
    }
}
