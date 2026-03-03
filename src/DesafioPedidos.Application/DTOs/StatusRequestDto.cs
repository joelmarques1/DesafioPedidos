using System.ComponentModel.DataAnnotations;

namespace DesafioPedidos.Application.DTOs;

public class StatusRequestDto
{
    [Required(ErrorMessage = "Status é obrigatório.")]
    public string Status { get; set; } = string.Empty;
    public int ItensAprovados { get; set; }
    public decimal ValorAprovado { get; set; }

    [Required(ErrorMessage = "Número do pedido é obrigatório.")]
    public string Pedido { get; set; } = string.Empty;
}
