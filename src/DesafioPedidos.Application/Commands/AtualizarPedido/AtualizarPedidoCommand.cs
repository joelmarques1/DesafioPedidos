using DesafioPedidos.Application.DTOs;

namespace DesafioPedidos.Application.Commands.AtualizarPedido;

public record AtualizarPedidoCommand(string NumeroPedido, List<ItemDto> NovosItens);
