using System.ComponentModel.DataAnnotations;

namespace DesafioPedidos.Application.DTOs;

public class PedidoRequestDto
{
    [Required(ErrorMessage = "Número do pedido é obrigatório.")]
    public string Pedido { get; set; } = string.Empty;

    [Required(ErrorMessage = "Itens são obrigatórios.")]
    [MinLength(1, ErrorMessage = "Pedido deve ter ao menos um item.")]
    public List<ItemDto> Itens { get; set; } = new();
}
