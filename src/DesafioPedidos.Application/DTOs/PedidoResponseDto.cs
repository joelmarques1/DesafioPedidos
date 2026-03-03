namespace DesafioPedidos.Application.DTOs;

public class PedidoResponseDto
{
    public string Pedido { get; set; } = string.Empty;
    public List<ItemDto> Itens { get; set; } = new();
    public decimal ValorTotal { get; set; }
    public int QuantidadeTotalItens { get; set; }
}
