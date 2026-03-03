using DesafioPedidos.Application.DTOs;
using DesafioPedidos.Domain.Exceptions;
using DesafioPedidos.Domain.Interfaces.Repositories;

namespace DesafioPedidos.Application.Queries.BuscarPedido;

public interface IBuscarPedidoHandler
{
    Task<PedidoResponseDto> HandleAsync(BuscarPedidoQuery query, CancellationToken cancellationToken = default);
}

public class BuscarPedidoHandler : IBuscarPedidoHandler
{
    private readonly IPedidoRepository _repository;

    public BuscarPedidoHandler(IPedidoRepository repository)
    {
        _repository = repository;
    }

    public async Task<PedidoResponseDto> HandleAsync(BuscarPedidoQuery query, CancellationToken cancellationToken = default)
    {
        var pedido = await _repository.BuscarPorNumeroAsync(query.NumeroPedido, cancellationToken)
            ?? throw new PedidoNaoEncontradoException(query.NumeroPedido);

        return new PedidoResponseDto
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
        };
    }
}
