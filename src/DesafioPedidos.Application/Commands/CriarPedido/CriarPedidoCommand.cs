using DesafioPedidos.Application.DTOs;

namespace DesafioPedidos.Application.Commands.CriarPedido;

public record CriarPedidoCommand(string NumeroPedido, List<ItemDto> Itens);
