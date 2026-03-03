using DesafioPedidos.Application.DTOs;
using DesafioPedidos.Domain.Entities;
using DesafioPedidos.Domain.Exceptions;
using DesafioPedidos.Domain.Interfaces.Repositories;

namespace DesafioPedidos.Application.Commands.CriarPedido;

public interface ICriarPedidoHandler
{
    Task<PedidoResponseDto> HandleAsync(CriarPedidoCommand command, CancellationToken cancellationToken = default);
}

public class CriarPedidoHandler : ICriarPedidoHandler
{
    private readonly IPedidoRepository _repository;

    public CriarPedidoHandler(IPedidoRepository repository)
    {
        _repository = repository;
    }

    public async Task<PedidoResponseDto> HandleAsync(CriarPedidoCommand command, CancellationToken cancellationToken = default)
    {
        if (await _repository.ExisteAsync(command.NumeroPedido, cancellationToken))
            throw new PedidoDuplicadoException(command.NumeroPedido);

        var itens = command.Itens
            .Select(i => new Item(i.Descricao, i.PrecoUnitario, i.Qtd))
            .ToList();

        var pedido = new Pedido(command.NumeroPedido, itens);
        await _repository.AdicionarAsync(pedido, cancellationToken);

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
