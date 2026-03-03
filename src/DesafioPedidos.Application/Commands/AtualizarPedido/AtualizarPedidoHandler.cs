using DesafioPedidos.Application.DTOs;
using DesafioPedidos.Domain.Entities;
using DesafioPedidos.Domain.Exceptions;
using DesafioPedidos.Domain.Interfaces.Repositories;

namespace DesafioPedidos.Application.Commands.AtualizarPedido;

public interface IAtualizarPedidoHandler
{
    Task<PedidoResponseDto> HandleAsync(AtualizarPedidoCommand command, CancellationToken cancellationToken = default);
}

public class AtualizarPedidoHandler : IAtualizarPedidoHandler
{
    private readonly IPedidoRepository _repository;

    public AtualizarPedidoHandler(IPedidoRepository repository)
    {
        _repository = repository;
    }

    public async Task<PedidoResponseDto> HandleAsync(AtualizarPedidoCommand command, CancellationToken cancellationToken = default)
    {
        var pedido = await _repository.BuscarPorNumeroAsync(command.NumeroPedido, cancellationToken)
            ?? throw new PedidoNaoEncontradoException(command.NumeroPedido);

        var novosItens = command.NovosItens
            .Select(i => new Item(i.Descricao, i.PrecoUnitario, i.Qtd))
            .ToList();

        pedido.AtualizarItens(novosItens);
        await _repository.AtualizarAsync(pedido, cancellationToken);

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
